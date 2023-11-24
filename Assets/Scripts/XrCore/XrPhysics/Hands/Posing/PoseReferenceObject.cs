using System;
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

        public void SetBoneVisibility(bool isVisible)
        {
            rightBones.drawBones = isVisible;
            leftBones.drawBones = isVisible;
        }

        public void SetBoneVisibility(bool isVisible, HandSide _side)
        {
            var targetRenderer = _side == HandSide.Left ? leftBones : rightBones;
            targetRenderer.drawBones = isVisible;
        }

        public void SetHandVisibility(bool isVisible, HandSide _side)
        {
            var targetRenderer = _side == HandSide.Left ? (leftHandPossable, rightHandPossable) : (rightHandPossable, leftHandPossable);
            targetRenderer.Item1.gameObject.SetActive(isVisible);
            targetRenderer.Item2.gameObject.SetActive(!isVisible);
        }

        public void SetHandTransform(Vector3 _position, Quaternion _rotation, HandSide _side)
        {
            var targetObj = _side == HandSide.Right ? rightHandPossable.gameObject : leftHandPossable.gameObject;
            targetObj.transform.position = _position;
            targetObj.transform.rotation = _rotation;
        }

        public void SetHandPose(HandPose _pose, HandSide _side)
        {
            if (_pose is null)
            {
                throw new ArgumentNullException(nameof(_pose));
            }

            var target = _side == HandSide.Right ? rightHandPossable : leftHandPossable;
            target.UpdateHandPose(_pose);
        }

        [ContextMenu("Apply Pose")]
        public void ApplyActivePose()
        {
            if (poseToApply != null) ApplyPose(poseToApply);
        }
    }
}
