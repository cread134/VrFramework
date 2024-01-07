using UnityEngine;
using XrCore.XrPhysics.Hands.Posing;
using XrCore.XrPhysics.Hands;
using XrCore.XrPhysics.Interaction;

namespace XrCore.XrPhysics.World
{
    public class StoredHandInformation
    {
        public StoredHandInformation(HandSide useHandSide, Transform initTransform)
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

        private HandSide handSide;
        private Transform storedTransform;
        private bool isGrabbing;

        public HandPose _handPose;
        public HandTransformReference transformReference;
        public GrabPoint heldPoint;
    }
}