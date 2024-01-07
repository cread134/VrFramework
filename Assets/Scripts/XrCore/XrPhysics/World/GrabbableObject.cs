using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XrCore.XrPhysics.Interaction;
using XrCore.XrPhysics.Hands;
using XrCore.XrPhysics.Hands.Posing;
using System.Linq;

namespace XrCore.XrPhysics.World
{
    [SelectionBase]
    [RequireComponent(typeof(Rigidbody))]
    public class GrabbableObject : MonoBehaviour, IGrabbable
    {
        private Rigidbody _rigidbody;

        public GrabPoint[] grabPoints;
        public XrObjectPhysicsConfig physicsSettings;
        private PhysicsMover _mover;

        private Dictionary<HandSide, StoredHandInformation> storedHandInformation;
        private HandSide primaryGrabSide = HandSide.Undetermined;

        bool IsTwoHanded 
            => storedHandInformation[HandSide.Right].IsGrabbingObject() && storedHandInformation[HandSide.Left].IsGrabbingObject();

        #region Setup
        private void Awake()
        {
            storedHandInformation = new Dictionary<HandSide, StoredHandInformation>
            {
                { HandSide.Left, new StoredHandInformation(HandSide.Left, transform) },
                { HandSide.Right, new StoredHandInformation(HandSide.Right, transform) },
            };
            SetupRigidbody();

            _mover = new PhysicsMover(physicsSettings, _rigidbody);

            foreach (var item in grabPoints)
            {
                item.OnSpawned(this);
            }
        }

        void SetupRigidbody()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
            _rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }

        #endregion

        public bool CanBeGrabbed(Vector3 grabPosition, Quaternion grabRotation, HandSide handType)
        {
            if (storedHandInformation[handType].IsGrabbingObject()) return false;
            return true;
        }

        public bool GetHandPosition(HandSide handType, Vector3 referecePosition, Vector3 forwardDirection, Vector3 upDirection, out Vector3 newPosition, out Quaternion newRotation)
        {
            if (storedHandInformation[handType].IsGrabbingObject())
            {
                return GetHandPositionGrabbing(handType, referecePosition, forwardDirection, upDirection, out newPosition, out newRotation);
            }
            else
            {
                return GetHandPositionNotGrabbing(handType, referecePosition, forwardDirection, upDirection, out newPosition, out newRotation);
            }
        }

        bool GetHandPositionGrabbing(HandSide handType, Vector3 referecePosition, Vector3 forwardDirection, Vector3 upDirection, out Vector3 newPosition, out Quaternion newRotation)
        {
            var transformValues = storedHandInformation[handType];
            Transform storedTransform = transformValues.GetStoredTransfromValues();
            newPosition = storedTransform.position;
            newRotation = storedTransform.rotation;
            Debug.DrawLine(referecePosition, newPosition, Color.cyan);

            storedHandInformation[handType]._handPose = transformValues.transformReference.GetTargetPose();
            return true;
        }

        bool GetHandPositionNotGrabbing(HandSide handType, Vector3 referecePosition, Vector3 forwardDirection, Vector3 upDirection, out Vector3 newPosition, out Quaternion newRotation)
        {
            var storedPossibleOrientations = new List<PossibleOrientation>();
            for (int i = 0; i < grabPoints.Length; i++)
            {
                var orientationInstance = new PossibleOrientation();

                orientationInstance.allowed = grabPoints[i].ToHandTransform(handType, referecePosition, forwardDirection, upDirection, out TransformOutPut outPut);
                orientationInstance.handTransformReference = outPut.referenceTransform;
                orientationInstance.orientationTransform = outPut.transform;
                orientationInstance.associatedPoint = grabPoints[i];
                storedPossibleOrientations.Add(orientationInstance);
            }

            var allowed = storedPossibleOrientations.Where(x => x.allowed == true);
            if (allowed == null || allowed.Count() == 0)
            {
                newPosition = Vector3.zero;
                newRotation = Quaternion.identity;
                return false;
            }
            foreach (var p in allowed)
            {
                Debug.Log(allowed.Count());
            }
            var match = allowed
                .OrderBy(x => (1f / Vector3.Distance(referecePosition, x.orientationTransform.position) * Quaternion.Angle(Quaternion.LookRotation(forwardDirection, upDirection), x.orientationTransform.rotation)))
                .First();

            Transform targetTransform = match.orientationTransform;
            newPosition = targetTransform.position;
            newRotation = targetTransform.rotation;
            storedHandInformation[handType].SetStoredTransform(match.orientationTransform);
            storedHandInformation[handType].heldPoint = match.associatedPoint;
            storedHandInformation[handType].transformReference = match.handTransformReference;
            storedHandInformation[handType]._handPose = storedHandInformation[handType].transformReference.GetTargetPose();
            Debug.DrawLine(referecePosition, newPosition, Color.magenta);
            return true;
        }

        struct PossibleOrientation
        {
            public bool allowed;
            public Transform orientationTransform;
            public HandTransformReference handTransformReference;
            public GrabPoint associatedPoint;
        }

        public StoredHandInformation GetHandInformation(HandSide handType)
        {
            return storedHandInformation[handType];
        }
        #region grabEvents 
        public void StartGrab(HandSide handType)
        {
            storedHandInformation[handType].SetGrabbing(true);
            if (primaryGrabSide == HandSide.Undetermined)
            {
                primaryGrabSide = handType;
            }
            _rigidbody.centerOfMass = _rigidbody.transform.InverseTransformPoint(storedHandInformation[primaryGrabSide].GetStoredTransfromValues().position);
            _rigidbody.useGravity = false;

            foreach (var item in GetSubscribers())
            {
                item.OnGripStarted(storedHandInformation[handType].heldPoint);
            }
        }
        public void OnGrabRelease(HandSide handType)
        {
            storedHandInformation[handType].SetGrabbing(false);

            if (primaryGrabSide == handType)
            {
                primaryGrabSide = HandSide.Undetermined;
            }

            CheckToResetGrabValues();

            foreach (var item in GetSubscribers())
            {
                item.OnGripFinished(storedHandInformation[handType].heldPoint);
            }
        }

        #endregion
        private void CheckToResetGrabValues()
        {
            foreach (HandSide item in Enum.GetValues(typeof(HandSide)))
            {
                if (storedHandInformation.ContainsKey(item) && storedHandInformation[item].IsGrabbingObject())
                {
                    return;
                }
            }
            _rigidbody.ResetCenterOfMass();
            if (_rigidbody.isKinematic) { _rigidbody.isKinematic = false; }
            _rigidbody.useGravity = true;
        }

        public void OnGrabTick()
        {
            //move to grab

            Transform grabPointTransform = storedHandInformation[primaryGrabSide].GetStoredTransfromValues();
            Vector3 calculatedTargetPosition = CalculatePositionalTarget(storedHandInformation[primaryGrabSide].targetPosition, grabPointTransform.position);
            Quaternion calculatedRotationTarget = IsTwoHanded ? CalculateTwoHandedRotation() 
                : CalculateRotationalTarget(storedHandInformation[primaryGrabSide].targetRotation, grabPointTransform.rotation);

            MatchHandWithPhysics(calculatedTargetPosition, calculatedRotationTarget);
        }

        private void MatchHandTransformWithoutPhysics(Vector3 newPosition, Quaternion newRotation)
        {
            if (!_rigidbody.isKinematic) _rigidbody.isKinematic = true;
            _rigidbody.MovePosition(newPosition);
            _rigidbody.MoveRotation(newRotation);
        }

        private void MatchHandWithPhysics(Vector3 newPosition, Quaternion newRotation)
        {
            if (_rigidbody.isKinematic) _rigidbody.isKinematic = false;
            PhysicsMatchHandPosition(newPosition);
            PhysicsMatchHandRotation(newRotation);
        }

        private void PhysicsMatchHandPosition(Vector3 targetPosition)
            => _mover.PhysicsMatchPosition(targetPosition);

        private void PhysicsMatchHandRotation(Quaternion targetRotation)
            => _mover.PhysicsMatchRotation(targetRotation);


        #region calculating target values

        /// <summary>
        /// Get the position we need to change the objects position to in order to match hand transform values
        /// </summary>
        /// <returns></returns>
        Vector3 CalculatePositionalTarget(Vector3 targetHandPosition, Vector3 targetGrabPosition)
        {
            Vector3 mainDifference = targetHandPosition - targetGrabPosition;
            return _rigidbody.position + mainDifference;
        }

        /// <summary>
        /// calculate rotation for two handed interactions
        /// </summary>
        /// <returns></returns>
        Quaternion CalculateTwoHandedRotation()
        {
            //two handed movement
            Transform mainGrabPoint = storedHandInformation[primaryGrabSide].GetStoredTransfromValues();
            HandSide oppositeSide = primaryGrabSide != HandSide.Left ? HandSide.Left : HandSide.Right;
            Transform secondaryGrabPoint = storedHandInformation[oppositeSide].GetStoredTransfromValues();
            Vector3 mainGrabTargetPosiiton = storedHandInformation[oppositeSide].targetPosition;

            Vector3 betweenVector = secondaryGrabPoint.position - mainGrabPoint.position;
            float mainGrabMagnitude = betweenVector.magnitude;
            Vector3 targetVector = (mainGrabTargetPosiiton - mainGrabPoint.position).normalized * mainGrabMagnitude;

            Quaternion dif = Quaternion.LookRotation(targetVector, storedHandInformation[primaryGrabSide].targetUpDirection) * Quaternion.Inverse(Quaternion.LookRotation(betweenVector, mainGrabPoint.up));
            Quaternion restulant = dif * transform.rotation;

            return restulant;
        }

        /// <summary>
        /// Get the rotation we need to change the objects rotation to in order to match hand transform values
        /// </summary>
        /// <returns></returns>
        Quaternion CalculateRotationalTarget(Quaternion targetHandRotation, Quaternion targetGrabRotation)
        {
            Quaternion C = targetHandRotation * Quaternion.Inverse(targetGrabRotation);
            Quaternion D = C * _rigidbody.rotation;
            return D;
        }
        #endregion

        private void OnDrawGizmosSelected()
        {
            if (grabPoints != null)
            {
                foreach (var grabPoint in grabPoints)
                {
                    Gizmos.color = new Color(1f, 0f, 1f, 0.2f);
                    Gizmos.DrawSphere(grabPoint.transform.position, 0.05f);

                    Gizmos.color = Color.red;
                    if (_rigidbody != null) Gizmos.DrawSphere(_rigidbody.centerOfMass, 0.05f);
                }
            }
        }

        public bool IsPrimaryGrab(HandSide handType)
        {
            return primaryGrabSide == handType;
        }

        public void UpdateTargetValues(HandSide handType, Vector3 targetPosition, Quaternion targetRotation, Vector3 targetUp)
        {
            storedHandInformation[handType].targetPosition = targetPosition;
            storedHandInformation[handType].targetRotation = targetRotation;
            storedHandInformation[handType].targetUpDirection = targetUp;
        }

        public HandPose GetTargetPose(HandSide handType)
        {
            return storedHandInformation[handType]._handPose;
        }

        #region transmittingEvents
        private List<IGrabObjectEvents> grabObjectEvents = new List<IGrabObjectEvents>();
        public void SubscribeEvents(IGrabObjectEvents events)
        {
            Debug.Log("subscribed to event");
            grabObjectEvents.Add(events);
        }

        public IGrabObjectEvents[] GetSubscribers()
        {
            return grabObjectEvents.ToArray();
        }
        #endregion
    }
}
