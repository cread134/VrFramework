using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace XrCore.XrPhysics.Hands.Posing
{
    public class PoseReferenceObject : MonoBehaviour
    {
        public PosableHandObject rightHandPossable;
        public PosableHandObject leftHandPossable;
        [SerializeField] private BoneRenderer rightBones;
        [SerializeField] private BoneRenderer leftBones;
        // [SerializeField] private 
        public PoseObject poseToApply;
        public void ApplyPose(PoseObject pose)
        {
            rightHandPossable.InitializeHand();
            leftHandPossable.InitializeHand();
            rightHandPossable.UpdateHandPose(pose.HandPose);
            leftHandPossable.UpdateHandPose(pose.HandPose);
        }

        public void SetBoneVisibility(bool isVisible, HandSide _side)
        {
            var targetRenderer = _side == HandSide.Left ? leftBones : rightBones;
            targetRenderer.drawBones = isVisible;
        }

        [ContextMenu("Apply Pose")]
        public void ApplyActivePose()
        {
            if (poseToApply != null) ApplyPose(poseToApply);
        }
    }
}
