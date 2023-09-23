using JetBrains.Annotations;
using OpenCover.Framework.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody))]
public class GrabbableObject : MonoBehaviour, IGrabbable
{
    private Rigidbody _rigidbody;

    [SerializeField] private GrabPoint[] grabPoints;

    private Dictionary<XrHand.HandSide, StoredHandInformation> storedHandInformation;

    [Header("Physics settings")]
    [SerializeField] private XrObjectPhysicsConfig physicsSettings;

    private XrHand.HandSide primaryGrabSide = XrHand.HandSide.Undetermined;

   
    private void Start()
    {
        storedHandInformation = new Dictionary<XrHand.HandSide, StoredHandInformation>
        {
            { XrHand.HandSide.Left, new StoredHandInformation(XrHand.HandSide.Left, transform) },
            { XrHand.HandSide.Right, new StoredHandInformation(XrHand.HandSide.Right, transform) }
        };
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        _rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    public bool CanBeGrabbed(Vector3 grabPosition, Quaternion grabRotation, XrHand.HandSide handType)
    {
        if (storedHandInformation[handType].IsGrabbingObject()) return false;
        return true;
    }

    public void GetHandPosition(XrHand.HandSide handType, Vector3 referecePosition, Vector3 forwardDirection, Vector3 upDirection, out Vector3 newPosition, out Quaternion newRotation)
    {
        if (storedHandInformation[handType].IsGrabbingObject())
        {
            Transform storedTransform = storedHandInformation[handType].GetStoredTransfromValues();
            newPosition = storedTransform.position;
            newRotation = storedTransform.rotation;

            if (storedTransform.gameObject.TryGetComponent<HandTransformReference>(out HandTransformReference transformReference))
            {
                storedHandInformation[handType]._handPose = transformReference.GetTargetPose();
            }
            Debug.DrawLine(newPosition, newPosition + Vector3.up, Color.red);
        }
        else
        {
            (float minValue, int index) matchingValues = (0f, 0);
            Transform[] storedPossibleOrientations = new Transform[grabPoints.Length];
            for (int i = 0; i < grabPoints.Length; i++)
            {
                storedPossibleOrientations[i] = grabPoints[i].ToHandTransform(handType, referecePosition, forwardDirection, upDirection);
                float distance = Vector3.Distance(referecePosition, storedPossibleOrientations[i].position);

                float matchScore = (1f / distance) * Quaternion.Angle(Quaternion.LookRotation(forwardDirection, upDirection), storedPossibleOrientations[i].rotation);

                if (matchScore > matchingValues.minValue)
                {
                    matchingValues.minValue = matchScore; 
                    matchingValues.index = i;
                }
            }
            Transform targetTransform = storedPossibleOrientations[matchingValues.index];
            newPosition = targetTransform.position;
            newRotation = targetTransform.rotation;
            storedHandInformation[handType].SetStoredTransform(storedPossibleOrientations[matchingValues.index]);
            if(targetTransform.gameObject.TryGetComponent<HandTransformReference>(out HandTransformReference transformReference))
            {
                storedHandInformation[handType]._handPose = transformReference.GetTargetPose();
            }
            
        }
    }


    public void StartGrab(XrHand.HandSide handType)
    {
        Debug.Log("startedGrab");
        storedHandInformation[handType].SetGrabbing(true);  
        if(primaryGrabSide == XrHand.HandSide.Undetermined)
        {
            primaryGrabSide = handType;
        }
        _rigidbody.centerOfMass = _rigidbody.transform.InverseTransformPoint(storedHandInformation[handType].GetStoredTransfromValues().position);
        _rigidbody.useGravity = false;  
    }
    public void OnGrabRelease(XrHand.HandSide handType)
    {
        Debug.Log("endedGrab");
        storedHandInformation[handType].SetGrabbing(false);

        if(primaryGrabSide == handType)
        {
            primaryGrabSide = XrHand.HandSide.Undetermined;
        }
      
        CheckToResetGrabValues();
    }

    private void CheckToResetGrabValues()
    {
        foreach (XrHand.HandSide item in Enum.GetValues(typeof(XrHand.HandSide)))
        {
            if (storedHandInformation.ContainsKey(item) && storedHandInformation[item].IsGrabbingObject())
            {
                return;
            }
        }
        _rigidbody.ResetCenterOfMass();
        if(_rigidbody.isKinematic) { _rigidbody.isKinematic = false; }
        _rigidbody.useGravity = true;
    }

    public void OnGrabTick()
    {
        //move to grab

        Transform grabPointTransform = storedHandInformation[primaryGrabSide].GetStoredTransfromValues();
        Vector3 calculatedTargetPosition = CalculatePositionalTarget(storedHandInformation[primaryGrabSide].targetPosition, grabPointTransform.position);
        Quaternion calculatedRotationTarget = Quaternion.identity;
        if (storedHandInformation[XrHand.HandSide.Right].IsGrabbingObject() && storedHandInformation[XrHand.HandSide.Left].IsGrabbingObject())
        {
            calculatedRotationTarget = CalculateTwoHandedRotation();
        }
        else
        {
             calculatedRotationTarget = CalculateRotationalTarget(storedHandInformation[primaryGrabSide].targetRotation, grabPointTransform.rotation);
        }
        Debug.DrawLine(calculatedTargetPosition, _rigidbody.position, Color.green);
        Debug.DrawLine(storedHandInformation[primaryGrabSide].targetPosition, _rigidbody.position, Color.red);
        MatchHandWithPhysics(calculatedTargetPosition, calculatedRotationTarget);
    }

    private void MatchHandTransformWithoutPhysics(Vector3 newPosition, Quaternion newRotation)
    {
        if(!_rigidbody.isKinematic)_rigidbody.isKinematic = true;
        _rigidbody.MovePosition(newPosition);
       _rigidbody.MoveRotation(newRotation);
    }

    private void MatchHandWithPhysics(Vector3 newPosition, Quaternion newRotation)
    {
        if (_rigidbody.isKinematic) _rigidbody.isKinematic = false;
         PhysicsMatchHandPosition(newPosition);
        PhysicsMatchHandRotation(newRotation);
    }

    Vector3 lastPositionDifference = Vector3.zero;
    Vector3 positionStoredIntegration;
    Vector3 lastTargetPosition;
    private void PhysicsMatchHandPosition(Vector3 targetPosition)
    {
        //handle position
        Vector3 positionError = targetPosition - _rigidbody.position;
        Vector3 proportion = positionError * physicsSettings.positionDifferenceMatchForce;

        Vector3 derivativeChange = (positionError - lastPositionDifference) / Time.fixedDeltaTime;
        lastPositionDifference = positionError;
        Vector3 velocityAdjustment =((lastTargetPosition - positionError) * Time.fixedDeltaTime) * physicsSettings.positionVelocityMultipler;
        _rigidbody.velocity *= (1f / physicsSettings.positionSmoothing);
        positionStoredIntegration += (positionError * Time.fixedDeltaTime) * physicsSettings.positionIntegrationCompenstation;
        Vector3 force = proportion + derivativeChange + positionStoredIntegration + velocityAdjustment;
        _rigidbody.AddForce(force, ForceMode.VelocityChange);
    }

    private Quaternion rotationError;
    private Quaternion lastRotation;
    private float angleError;
    private Vector3 errorAxis;
    private void PhysicsMatchHandRotation(Quaternion targetRotation)
    {
        rotationError = targetRotation * Quaternion.Inverse(_rigidbody.rotation);

        rotationError.ToAngleAxis(out angleError, out errorAxis);
        errorAxis.Normalize();
        if (angleError > 180f)
        {
            angleError -= 360f;
        }

        if (Quaternion.Angle(targetRotation, _rigidbody.rotation) < physicsSettings.angleAllowance)
        {
            _rigidbody.MoveRotation(Quaternion.Slerp(_rigidbody.rotation, targetRotation, Time.deltaTime * physicsSettings.rotationalSmoothing));
            _rigidbody.angularVelocity = Vector3.zero;
        }
        else
        {
            Quaternion rotationImpulse = lastRotation * Quaternion.Inverse(targetRotation);
            rotationImpulse.ToAngleAxis(out float angleImpluse, out Vector3 impulseAxis);

            _rigidbody.angularVelocity *= physicsSettings.anglularSlowdown;

            Vector3 angularVelocity = (errorAxis * angleError) * Time.deltaTime * physicsSettings.rotationMultiplier;
            Vector3 angularImpulse = (impulseAxis * angleImpluse) * Time.fixedDeltaTime * physicsSettings.rotationImpulseCompensation;

            angularVelocity += (_rigidbody.angularVelocity - angularVelocity) * physicsSettings.rotationProportionalGain * Time.deltaTime;
            angularVelocity += angularImpulse;
            _rigidbody.AddTorque(angularVelocity, ForceMode.VelocityChange);
        }

        lastRotation = targetRotation;
    }


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
        XrHand.HandSide oppositeSide = primaryGrabSide != XrHand.HandSide.Left ? XrHand.HandSide.Left : XrHand.HandSide.Right;
        Transform secondaryGrabPoint = storedHandInformation[oppositeSide].GetStoredTransfromValues();
        Vector3 mainGrabTargetPosiiton = storedHandInformation[oppositeSide].targetPosition; 

        Vector3 betweenVector = secondaryGrabPoint.position - mainGrabPoint.position;
        float mainGrabMagnitude = betweenVector.magnitude;
        Vector3 targetVector = (mainGrabTargetPosiiton - mainGrabPoint.position).normalized * mainGrabMagnitude;

        Debug.DrawLine(mainGrabPoint.position, mainGrabPoint.position + betweenVector, Color.red);
        Debug.DrawLine(mainGrabPoint.position, mainGrabPoint.position + targetVector, Color.green);

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
                if(_rigidbody != null) Gizmos.DrawSphere(_rigidbody.centerOfMass, 0.05f);
            }
        }
    }

    public bool IsPrimaryGrab(XrHand.HandSide handType)
    {
        return primaryGrabSide == handType;
    }

    public void UpdateTargetValues(XrHand.HandSide handType, Vector3 targetPosition, Quaternion targetRotation, Vector3 targetUp)
    {
        storedHandInformation[handType].targetPosition = targetPosition;
        storedHandInformation[handType].targetRotation = targetRotation;
        storedHandInformation[handType].targetUpDirection = targetUp;
    }

    public void OnMainButtonDown(XrHand.HandSide handType)
    {
       
    }

    public void OnMainButtonUp(XrHand.HandSide handType)
    {

    }

    public void TriggerDown(XrHand.HandSide handType)
    {
      
    }

    public void TriggerUp(XrHand.HandSide handType)
    {
        
    }

    public HandPose GetTargetPose(XrHand.HandSide handType)
    {
        return storedHandInformation[handType]._handPose;
    }

    public class StoredHandInformation
    {
        public StoredHandInformation(XrHand.HandSide useHandSide, Transform initTransform) 
        {
            handSide = useHandSide;
            storedTransform = initTransform;
            isGrabbing = false;
        }

        public Vector3 targetUpDirection;
        public Vector3 targetPosition;
        public Quaternion targetRotation;

        public bool IsGrabbingObject()
        {
            return isGrabbing;
        }

        public void SetGrabbing(bool value)
        {
            isGrabbing = value; 
        }

        public void SetStoredTransform(Transform value)
        {
            storedTransform = value;
        }

        public Transform GetStoredTransfromValues()
        {
            return storedTransform;
        }

        private XrHand.HandSide handSide;
        private Transform storedTransform;
        private bool isGrabbing;

        public HandPose _handPose;
    }
    
}