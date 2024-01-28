using Core.DI;
using System.Linq;
using UnityEngine;
using XrCore.Interaction.Control;
using XrCore.XrPhysics.Hands.Posing;
using XrCore.XrPhysics.World;

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

        private PhysicsMover _mover;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            loggingService = DependencyService.Resolve<ILoggingService>();

            _mover = new PhysicsMover(physicsConfig, rb);
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
            isGrabbingObject = false;

            SetHandCollision(true);
        }

        private void OnGripUpdate()
        {
            if (!isGrabbingObject)
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
            if (!isGrabbingObject)
            {
                MoveHand();
            }

            if (!isGrabbingObject)
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
        private bool isGrabbingObject;
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
                    bool allowed = grabbable.GetHandPosition(handType, transform.position, transform.forward, transform.up, out Vector3 targetPos, out Quaternion targetRot);
                    if (Vector3.Distance(grabCentre.position, targetPos) < grabRadius && allowed)
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

        public void GrabFromPoint(HandTransformReference handPoint)
        {
            IGrabbable grabbable = handPoint.GrabParent;
            bool allowed = grabbable.GetHandPosition(handType, handPoint.transform.position, handPoint.transform.forward, handPoint.transform.up, out Vector3 targetPos, out Quaternion targetRot);
            if(!allowed)
            {
                Debug.Log("no possible grab point found");
                return;
            }
            grabHover = grabbable;
            overTarget = true;
            trackingTarget.transform.position = targetPos;
            trackingTarget.transform.rotation = targetRot;
            UpdateGrip(1f);
            handClosed = true;
            StartGrab();
        }
        public void StartGrab()
        {
            loggingService.Log("started Grabbing " + grabHover.ToString());
            grabHover.StartGrab(handType);
            currentGrab = grabHover;
            isGrabbingObject = true;
            poseHand.UpdateHandPose(currentGrab.GetTargetPose(handType));
            SetHandCollision(false);
        }

        private void SetHandCollision(bool collisionOn)
        {
            handCollider.SetColliderActive(collisionOn);
        }

        private void UpdateGrabbedObject()
        {
            if (isGrabbingObject && currentGrab != null)
            {
                currentGrab.UpdateTargetValues(handType, trackingTarget.position, trackingTarget.rotation, trackingTarget.up);
                if (currentGrab.IsPrimaryGrab(handType))
                {
                    currentGrab.OnGrabTick();
                }
                currentGrab.GetHandPosition(handType, trackingTarget.position, trackingTarget.forward, trackingTarget.up, out Vector3 newPosition, out Quaternion newRotation);

                if(currentGrab.BreakGripOnDistance)
                {
                    var currentDistance = Vector3.Distance(newPosition, trackingTarget.position);
                    if(currentDistance > currentGrab.BreakDistance)
                    {
                        ReleaseGrab();
                        return;
                    }
                }

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
            _mover.PhysicsMatchPosition(trackingTarget.position);
        }

        private void PhysicsMatchHandRotation()
        {
            _mover.PhysicsMatchRotation(trackingTarget.rotation);
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

            if(CanGrab)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(grabCentre.position, grabRadius);
            }
        }

        #region InputEvents
        public void UpdateGrip(float newValue)
        {
            float oldvalue = m_gripValue;
            m_gripValue = newValue;
            OnGripUpdate();
            if(isGrabbingObject)
            {
                var eventSubscribers = currentGrab.GetSubscribers();
                foreach (var item in eventSubscribers)
                {
                    item.OnGripChange(newValue, oldvalue, currentGrab.GetHandInformation(handType).heldPoint);
                }
            }
        }

        public void UpdateTrigger(float newValue)
        {
            float oldValue = m_triggerValue;

            if (!isGrabbingObject) return;
            var eventSubscribers = currentGrab.GetSubscribers();
            foreach (var item in eventSubscribers)
            {
                item.OnGripChange(newValue, oldValue, currentGrab.GetHandInformation(handType).heldPoint);
            }
        }

        public float ReadGrip() => m_gripValue;
        public float ReadTrigger() => m_triggerValue;

        public void OnMainButtonDown()
        {
            if (!isGrabbingObject) return;
            var eventSubscribers = currentGrab.GetSubscribers();
            foreach (var item in eventSubscribers)
            {
                item.OnMainDown(currentGrab.GetHandInformation(handType).heldPoint);
            }
        }

        public void OnMainButtonUp()
        {
            if (!isGrabbingObject) return;
            var eventSubscribers = currentGrab.GetSubscribers();
            foreach (var item in eventSubscribers)
            {
                item.OnMainUp(currentGrab.GetHandInformation(handType).heldPoint);
            }
        }

        public void OnSecondaryButtonDown()
        {

        }

        public void OnSecondaryButtonUp()
        {

        }

        public void OnGripDown()
        {
            if (!handClosed)
            {
                OnGrab();
            }
        }

        public void OnGripUp()
        {
            if (handClosed)
            {
                OnRelease();
            }
        }

        public void OnTriggerDown()
        {
            if (!isGrabbingObject) return;
            var eventSubscribers = currentGrab.GetSubscribers();
            foreach (var item in eventSubscribers)
            {
                item.OnTriggerDown(currentGrab.GetHandInformation(handType).heldPoint);
            }
        }

        public void OnTriggerUp()
        {
            if (!isGrabbingObject) return;
            var eventSubscribers = currentGrab.GetSubscribers();
            foreach (var item in eventSubscribers)
            {
                item.OnTriggerUp(currentGrab.GetHandInformation(handType).heldPoint);
            }
        }
        #endregion
    }
}