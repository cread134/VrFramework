using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XrCore.XrPhysics.Hands;
using XrCore.XrPhysics.Hands.Posing;

public class GrabbableObjectController : MonoBehaviour, IGrabbable
{
    private IGrabbable targetObject;
    [SerializeField] private GameObject _targetObject;

    private void Awake()
    {
        targetObject = _targetObject.GetComponent<IGrabbable>();
    }
    public bool CanBeGrabbed(Vector3 grabPosition, Quaternion grabRotation, HandSide handType)
    {
        return targetObject.CanBeGrabbed(grabPosition, grabRotation, handType);
    }

    public void GetHandPosition(HandSide handType, Vector3 referecePosition, Vector3 forwardDirection, Vector3 upDirection, out Vector3 newPosition, out Quaternion newRotation)
    {
        targetObject.GetHandPosition(handType, referecePosition, forwardDirection, upDirection, out Vector3 _newPosition, out Quaternion _newRotation);
        newPosition = _newPosition;
        newRotation = _newRotation;
    }

    public bool IsPrimaryGrab(HandSide handType)
    {
        return targetObject.IsPrimaryGrab(handType);
    }

    public void OnGrabRelease(HandSide handType)
    {
        targetObject.OnGrabRelease(handType);
    }

    public void OnGrabTick()
    {
        targetObject.OnGrabTick();
    }

    public void OnMainButtonDown(HandSide handType)
    {
        targetObject.OnMainButtonDown(handType);
    }

    public void OnMainButtonUp(HandSide handType)
    {
        targetObject.OnMainButtonUp(handType);
    }

    public void StartGrab(HandSide handType)
    {
       targetObject.StartGrab(handType);
    }

    public void TriggerDown(HandSide handType)
    {
        targetObject.TriggerDown(handType);
    }

    public void TriggerUp(HandSide handType)
    {
        targetObject.TriggerUp(handType);
    }

    public void UpdateTargetValues(HandSide handType, Vector3 targetPosition, Quaternion targetRotation, Vector3 targetUp)
    {
        targetObject.UpdateTargetValues(handType, targetPosition, targetRotation, targetUp);
    }

    public HandPose GetTargetPose(HandSide handType)
    {
        throw new System.NotImplementedException();
    }
}
