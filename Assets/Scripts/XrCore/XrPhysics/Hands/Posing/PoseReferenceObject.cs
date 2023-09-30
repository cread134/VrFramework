using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XrCore.XrPhysics.Hands.Posing
{
    public class PoseReferenceObject : MonoBehaviour
    {
        public PosableHandObject rightHandPossable;
        public PosableHandObject leftHandPossable;
        public PoseObject poseToApply;
        public void ApplyPose(PoseObject pose)
        {
            rightHandPossable.InitializeHand();
            leftHandPossable.InitializeHand();
            Debug.Log(poseToApply.GetValues()); ;
            rightHandPossable.UpdateHandPose(pose.HandPose);
            leftHandPossable.UpdateHandPose(pose.HandPose);
        }

        [ContextMenu("Apply Pose")]
        public void ApplyActivePose()
        {
            if (poseToApply != null) ApplyPose(poseToApply);
        }
    }
}
