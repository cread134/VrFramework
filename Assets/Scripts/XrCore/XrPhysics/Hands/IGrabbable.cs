using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XrCore.XrPhysics.Hands.Posing;
using XrCore.XrPhysics.World;

namespace XrCore.XrPhysics.Hands
{
    public interface IGrabbable
    {
        public bool CanBeGrabbed(Vector3 grabPosition, Quaternion grabRotation, HandSide handType);

        public void StartGrab(HandSide handType);

        public void UpdateTargetValues(HandSide handType, Vector3 targetPosition, Quaternion targetRotation, Vector3 targetUp);

        public void OnGrabTick();

        public void GetHandPosition(HandSide handType, Vector3 referecePosition, Vector3 forwardDirection, Vector3 upDirection, out Vector3 newPosition, out Quaternion newRotation);

        public void OnGrabRelease(HandSide handType);

        public bool IsPrimaryGrab(HandSide handType);

        public HandPose GetTargetPose(HandSide handType);
        public IGrabObjectEvents[] GetSubscribers();
    }
}
