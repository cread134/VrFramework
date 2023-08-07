using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class XrHand : MonoBehaviour
{
    private Rigidbody rb;

    public enum HandSide { Left, Right, Undetermined };
    public HandSide handType;

    [Header("Physics hand settings")]
    [SerializeField] private Transform trackingTarget;
    [SerializeField] private PosableHandObject poseHand;
    [SerializeField] private PoseObject idlePose;
    [SerializeField] private PoseObject gripPose;

    public Transform TrackingTarget() { return trackingTarget; }

    [Space]
    [SerializeField] private Transform grabCentre;
    [SerializeField] private LayerMask grabMask;
    [SerializeField] private float grabRadius = 0.05f;
    [Space]
    [SerializeField] private LayerMask collisionMask;
    [SerializeField] private float collisionRadius = 0.1f;
    [Space]
    [SerializeField] private float positionProportionalGain = 8f; // Proportional gain for position control
    [SerializeField] private float positionIntegralGain = 8f;
    [SerializeField] private float positionDerivativeGain = 8f;
    [Space]
    [SerializeField] private float rotationProportionalGain = 8f; // Proportional gain for rotation control
    [SerializeField] private float torqueDamping = 1f;
    [SerializeField] private float rotationMultiplier = 8f;
    [SerializeField] private float angleAllowance = 5f;
    [SerializeField] private float rotationalSmoothing = 12f;
    [SerializeField][Range(0f, 1f)] private float anglularSlowdown = 0.7f;
    [Space]
    [SerializeField] private Collider handCollider;

    private float m_gripValue = 0f;
    private float m_triggerValue = 0f;


    [Header("Grabbing settings")]
    [SerializeField] private float grabThreshold = 0.8f;
    private bool handClosed = false;

    [Header("Visuals")]
    [SerializeField] private GameObject grabIndicator;

    //pid values
    private Vector3 positionError;
    private Vector3 lastPositionError;
    private Vector3 positionStoredIntegration;

    private Quaternion rotationError;
    private float angleError;
    private Vector3 errorAxis;


    private bool inPhysicsRange;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        gripPose.CachePose();
        idlePose.CachePose();
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
            if(handClosed)
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
        if(currentGrab != null)
        {
            currentGrab.OnGrabRelease(handType);
        }
        currentGrab = null;
        isGrabbing = false;

        SetHandCollision(true);
    }

    private void OnGripUpdate()
    {
        if(!isGrabbing)
        {
            poseHand.LerpPose(idlePose.GetPose(), gripPose.GetPose(), m_gripValue);
        }
    }
    private void Update()
    {
        if (!isGrabbing)
        {
            MoveHand();
        }

        if(!handClosed)
        {
            ObserveGrabbable();
        }
        else
        {
            UpdateGrabbedObject();
        }
    }

# region grabbing

    private bool overTarget = false;
    private IGrabbable grabHover;
    private IGrabbable currentGrab;
    private bool isGrabbing;
    private void ObserveGrabbable()
    {
        Collider[] found = Physics.OverlapSphere(grabCentre.position, grabRadius, grabMask, QueryTriggerInteraction.Ignore);

        if(found != null && found.Length > 0)
        {
            Collider closest = found[0];
            foreach(Collider c in found)
            {
                if(Vector3.Distance(c.transform.position, grabCentre.position) < Vector3.Distance(closest.transform.position, grabCentre.position)){
                    closest = c;
                }
            }

            //observe grabbalbe
            if(closest.gameObject.TryGetComponent<IGrabbable>(out IGrabbable grabbable) && grabbable.CanBeGrabbed(transform.position, transform.rotation, handType))
            {
                grabbable.GetHandPosition(handType, transform.position, transform.forward, transform.up, out Vector3 targetPos, out Quaternion targetRot);
                if (Vector3.Distance(grabCentre.position, targetPos) < grabRadius)
                {
                    grabHover = grabbable;

                    Debug.DrawLine(closest.transform.position, grabCentre.position, Color.green);
                    grabIndicator.transform.position = targetPos;
                    grabIndicator.SetActive(true);

                    overTarget = true;
                }
            }
            else
            {
                grabIndicator.SetActive(false);
                overTarget = false;
            }
        }
        else
        {
            grabIndicator.SetActive(false);
            overTarget = false;
        }
    }

    private void AttemptGrab()
    {
        if (overTarget && grabHover != null)
        {
            StartGrab();
        }
    }

    private void StartGrab()
    {
        grabHover.StartGrab(handType);
        currentGrab = grabHover;
        isGrabbing = true;
        grabIndicator.SetActive(false);
        poseHand.UpdateHandPose(currentGrab.GetTargetPose(handType));
        SetHandCollision(false);
    }

    private void SetHandCollision(bool collisionOn)
    {
        //handCollider.enabled = collisionOn;
        if (collisionOn)
        {
            handCollider.gameObject.layer = 10;
        }
        else
        {
            handCollider.gameObject.layer = 11;
        }
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
        if (inPhysicsRange)
        {         //handle rotation
            PhysicsMatchHandPosition();
            PhysicsMatchHandRotation();
        }
        else
        {
            MoveHandKinematic();
        }
    }

    private void PhysicsMatchHandPosition()
    {
        //handle position
        positionError = trackingTarget.position - rb.position;

        Vector3 positionProportion = positionError * positionProportionalGain;

        Vector3 derivativeGain = (positionError - lastPositionError) / Time.fixedDeltaTime;
        Vector3 positionDerivative = derivativeGain * positionDerivativeGain;

        lastPositionError = positionError;

        positionStoredIntegration += (positionError * Time.fixedDeltaTime);

        Vector3 force = positionProportion + positionStoredIntegration + positionDerivative;
       // Debug.Log("pid force " + force);
        rb.AddForce(force);
    }


    private void PhysicsMatchHandRotation()
    {
        rotationError = trackingTarget.rotation * Quaternion.Inverse(rb.rotation);

        rotationError.ToAngleAxis(out angleError, out errorAxis);
        errorAxis.Normalize();
        if (angleError > 180f)
        {
            angleError -= 360f;
        }

        if (Quaternion.Angle(trackingTarget.rotation, rb.rotation) < angleAllowance)
        {
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, trackingTarget.rotation, Time.deltaTime * rotationalSmoothing));
            rb.angularVelocity = Vector3.zero;
        }
        else
        {
            rb.angularVelocity *= anglularSlowdown * (1f - (angleError / 360f));

            Vector3 angularVelocity = (errorAxis * angleError) / Time.deltaTime;

            angularVelocity -= rb.angularVelocity * torqueDamping;
            angularVelocity += (angularVelocity - rb.angularVelocity) * rotationProportionalGain;
            float ratio = 1f + (angleError / 360f);
            rb.AddTorque((angularVelocity - rb.angularVelocity) * Time.deltaTime * rotationMultiplier * (ratio * ratio), ForceMode.VelocityChange);
        }
       // rb.angularVelocity = angularVelocity;
    }

    private void MoveHandKinematic()
    {
        if (!rb.isKinematic) rb.isKinematic = true;
        rb.MovePosition(trackingTarget.position);
        rb.MoveRotation(trackingTarget.rotation);

        positionStoredIntegration = Vector3.zero;
        lastPositionError = Vector3.zero;
    }

    #endregion

    private void OnDrawGizmos()
    {
        if(grabCentre != null)
        {
            Gizmos.color = Color.Lerp(Color.green, Color.red, m_gripValue);
            Gizmos.DrawWireSphere(grabCentre.position, grabRadius);
        }

       

        if(trackingTarget != null)
        {
            Gizmos.color = Color.red;
           // Gizmos.DrawWireSphere (trackingTarget.position, grabRadius);
            Gizmos.DrawLine(transform.position, trackingTarget.position);

            Gizmos.color = Color.black;
            Gizmos.DrawSphere(trackingTarget.transform.position, 0.01f);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(trackingTarget.position, trackingTarget.position + (trackingTarget.forward * 0.03f));
            Gizmos.color = Color.green;
            Gizmos.DrawLine(trackingTarget.position, trackingTarget.position + (trackingTarget.up * 0.03f));
            Gizmos.color = Color.red;
            Gizmos.DrawLine(trackingTarget.position, trackingTarget.position + (trackingTarget.right * 0.03f));
        }

        if (!inPhysicsRange)
        {
            Gizmos.color = new Color(1f, 0, 0f, 0.3f);
            Gizmos.DrawWireSphere(transform.position, collisionRadius);
        }
    }

    private void OnMouseDown()
    {
        DebugController.Instance.SelectController(this);
    }

}
