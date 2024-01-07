using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XrCore.XrPhysics.Hands;
using XrCore.XrPhysics.Hands.Posing;

namespace XrCore.XrPhysics.World
{
    public class ChildGrabbable : MonoBehaviour, IGrabbable
    {
        [SerializeField] private GrabbableObject grabbableObject;

        public bool CanBeGrabbed(Vector3 grabPosition, Quaternion grabRotation, HandSide handType)
            => grabbableObject.CanBeGrabbed(grabPosition, grabRotation, handType);

        public StoredHandInformation GetHandInformation(HandSide handType)
            => grabbableObject.GetHandInformation(handType);

        public bool GetHandPosition(HandSide handType, Vector3 referecePosition, Vector3 forwardDirection, Vector3 upDirection, out Vector3 newPosition, out Quaternion newRotation)
            => grabbableObject.GetHandPosition(handType, referecePosition, forwardDirection, upDirection, out newPosition, out newRotation);

        public IGrabObjectEvents[] GetSubscribers()
            => grabbableObject.GetSubscribers();

        public HandPose GetTargetPose(HandSide handType)
            => grabbableObject.GetTargetPose(handType);

        public bool IsPrimaryGrab(HandSide handType)
            => grabbableObject.IsPrimaryGrab(handType);

        public void OnGrabRelease(HandSide handType)
            => grabbableObject.OnGrabRelease(handType);

        public void OnGrabTick()
            => grabbableObject.OnGrabTick();

        public void StartGrab(HandSide handType)
            => grabbableObject.StartGrab(handType);

        public void UpdateTargetValues(HandSide handType, Vector3 targetPosition, Quaternion targetRotation, Vector3 targetUp)
            => grabbableObject.UpdateTargetValues(handType, targetPosition, targetRotation, targetUp);

    }
}