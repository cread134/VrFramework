using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGrabbable
{
    public bool CanBeGrabbed(Vector3 grabPosition, Quaternion grabRotation, XrHand.HandSide handType);

    public void StartGrab(XrHand.HandSide handType);

    public void UpdateTargetValues(XrHand.HandSide handType, Vector3 targetPosition, Quaternion targetRotation, Vector3 targetUp);

    public void OnGrabTick();

    public void GetHandPosition(XrHand.HandSide handType, Vector3 referecePosition, Vector3 forwardDirection, Vector3 upDirection, out Vector3 newPosition, out Quaternion newRotation);

    public void OnGrabRelease(XrHand.HandSide handType);

    public bool IsPrimaryGrab(XrHand.HandSide handType);

    public void OnMainButtonDown(XrHand.HandSide handType);

    public void OnMainButtonUp(XrHand.HandSide handType);

    public void TriggerDown(XrHand.HandSide handType);

    public void TriggerUp(XrHand.HandSide handType);

    public HandPose GetTargetPose(XrHand.HandSide handType);
}
