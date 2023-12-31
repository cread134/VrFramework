using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using XrCore.XrPhysics.Hands.Posing;
using XrCore.Interaction.Control;
using TMPro;
using Core.DI;
using Core.Logging;
using XrCore.XrPhysics.World;
using System.Linq;

namespace XrCore.XrPhysics.Hands
{
    public enum HandSide { Left, Right, Undetermined };

    public class XrHand : MonoBehaviour, IXrHandControls
    {
        private Rigidbody rb;
        private ILoggingService loggingService;
        public HandSide handType;

        [Header("Physics hand settings")]
        [SerializeField] private Transform trackingTarget;
        [SerializeField] private PosableHandObject poseHand;
        [SerializeField] private PoseObject idlePose;
        [SerializeField] private PoseObject gripPose;

        public Transform TrackingTarget() { return trackingTarget; }

        [SerializeField] private XrObjectPhysicsConfig physicsConfig;
        [Space]
        [SerializeField] private float maxAllowedHandDistance = 5f;
        [SerializeField] private Transform grabCentre;
        [SerializeField] private LayerMask grabMask;
        [SerializeField] private float grabRadius = 0.05f;
        [Space]
        [SerializeField] private LayerMask collisionMask;
        [SerializeField] private float collisionRadius = 0.1f;
        [Space]
        [SerializeField] private HandCollider handCollider;

        private float m_gripValue = 0f;
        private float m_triggerValue = 0f;


        [Header("Grabbing settings")]
        [SerializeField] private float grabThreshold = 0.8f;
        private bool handClosed = false;

        private bool inPhysicsRange;
        private PhysicsMover _mover;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            loggingService = DependencyService.Resolve<ILoggingService>();

            _mover = new PhysicsMover(physicsConfig, rb);
        }

        public float ReadGripValue() { return m_gripValue; }
        public void UpdateGripValue(float value)
        {
            m_gripValue = value;
            if (m_gripValue > grabThreshold)
            {
                if (!handClosed)
                {
                    OnGrab();
                }
            }
            else
            {
                if (handClosed)
                {
                    OnRelease();
                }
            }
            OnGripUpdate();
        }

        private void OnGrab()
        {
            AttemptGrab();
            handClosed = true;
        }

        private void OnRelease()
        {
            handClosed = false;
            ReleaseGrab();
        }

        private void ReleaseGrab()
        {
            if (currentGrab != null)
            {
                currentGrab.OnGrabRelease(handType);
            }
            currentGrab = null;
            isGrabbing = false;

            SetHandCollision(true);
        }

        private void OnGripUpdate()
        {
            if (!isGrabbing)
            {
                var poseIdle = idlePose.HandPose;
                var poseGrip = gripPose.HandPose;
                if (poseIdle is null || poseGrip is null)
                {
                    return;
                }
                poseHand.LerpPose(idlePose.HandPose, gripPose.HandPose, m_gripValue);
            }
        }
        private void Update()
        {
            if (!isGrabbing)
            {
                MoveHand();
            }

            if (!handClosed)
            {
                ObserveGrabbable();
            }
            else
            {
                UpdateGrabbedObject();
            }
        }

        #region grabbing

        private bool overTarget = false;
        private IGrabbable grabHover;
        private IGrabbable currentGrab;
        private bool isGrabbing;
        private void ObserveGrabbable()
        {
            var found = Physics.OverlapSphere(grabCentre.position, grabRadius, grabMask, QueryTriggerInteraction.Ignore);

            if (found != null && found.Length > 0)
            {
                var first = found.OrderBy(c => Vector3.Distance(c.transform.position, grabCentre.position))?
                    .ToArray()?
                    .First();

                if (first.gameObject.TryGetComponent<IGrabbable>(out IGrabbable grabbable) && grabbable.CanBeGrabbed(transform.position, transform.rotation, handType))
                {
                    grabbable.GetHandPosition(handType, transform.position, transform.forward, transform.up, out Vector3 targetPos, out Quaternion targetRot);
                    if (Vector3.Distance(grabCentre.position, targetPos) < grabRadius)
                    {
                        grabHover = grabbable;
                        overTarget = true;
                    }
                }
                else
                {
                    overTarget = false;
                }
            }
            else
            {
                overTarget = false;
            }
        }

        bool CanGrab => overTarget && grabHover != null;
        private void AttemptGrab()
        {
            if (CanGrab)
            {
                StartGrab();
            }
        }

        private void StartGrab()
        {
            loggingService.Log("started Grabbing " + grabHover.ToString());
            grabHover.StartGrab(handType);
            currentGrab = grabHover;
            isGrabbing = true;
            poseHand.UpdateHandPose(currentGrab.GetTargetPose(handType));
            SetHandCollision(false);
        }

        private void SetHandCollision(bool collisionOn)
        {
            handCollider.SetColliderActive(collisionOn);
        }

        private void UpdateGrabbedObject()
        {
            if (isGrabbing && currentGrab != null)
            {
                currentGrab.UpdateTargetValues(handType, trackingTarget.position, trackingTarget.rotation, trackingTarget.up);
                if (currentGrab.IsPrimaryGrab(handType))
                {
                    currentGrab.OnGrabTick();
                }
                currentGrab.GetHandPosition(handType, trackingTarget.position, trackingTarget.forward, trackingTarget.up, out Vector3 newPosition, out Quaternion newRotation);
                if (!rb.isKinematic) rb.isKinematic = true;

                rb.MovePosition(newPosition);
                rb.MoveRotation(newRotation);
            }
        }



        #endregion

        #region handMovement
        private void MoveHand()
        {
            if (rb.isKinematic) rb.isKinematic = false;
            inPhysicsRange = Physics.CheckSphere(transform.position, collisionRadius, collisionMask, QueryTriggerInteraction.Ignore);
            if (Vector3.Distance(transform.position, trackingTarget.position) > maxAllowedHandDistance)
            {
                MoveHandKinematic();
            }
            else
            {
                MoveHandPhysics();
            }
        }

        private void MoveHandPhysics()
        {
            PhysicsMatchHandPosition();
            PhysicsMatchHandRotation();
        }

        private void PhysicsMatchHandPosition()
        {
            _mover.PhysicsMatchHandPosition(trackingTarget.position);
        }

        private void PhysicsMatchHandRotation()
        {
            _mover.PhysicsMatchHandRotation(trackingTarget.rotation);
        }

        private void MoveHandKinematic()
        {
            if (!rb.isKinematic) rb.isKinematic = true;
            rb.MovePosition(trackingTarget.position);
            rb.MoveRotation(trackingTarget.rotation);
        }

        #endregion

        private void OnDrawGizmos()
        {
            if (grabCentre != null)
            {
                Gizmos.color = Color.Lerp(Color.green, Color.red, m_gripValue);
                Gizmos.DrawWireSphere(grabCentre.position, grabRadius);
            }

            if (!inPhysicsRange)
            {
                Gizmos.color = new Color(1f, 0, 0f, 0.3f);
                Gizmos.DrawWireSphere(transform.position, collisionRadius);
            }

            if(CanGrab)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(grabCentre.position, grabRadius);
            }
        }


        public void UpdateGrip(float newValue) => UpdateGripValue(newValue);

        public void UpdateTrigger(float newValue)
        {

        }

        public void OnMainButtonDown()
        {

        }

        public void OnMainButtonUp()
        {

        }

        public void OnSecondaryButtonDown()
        {

        }

        public void OnSecondaryButtonUp()
        {
            throw new System.NotImplementedException();
        }
    }
}
