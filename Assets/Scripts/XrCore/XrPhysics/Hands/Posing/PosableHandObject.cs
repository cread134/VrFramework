using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
namespace XrCore.XrPhysics.Hands.Posing
{
    public class PosableHandObject : MonoBehaviour
    {
        private XrHandBones handBones;
        private XrHandBones HandBones
        {
            get
            {
                if (handBones == null)
                {
                    var bones = new XrHandBones(boneTransforms);
                    handBones = bones;
                    return bones;
                }
                return handBones;
            }
        }

        public HandSide handType;
        [SerializeField] private List<Transform> boneTransforms;


        private void Start()
        {
           InitializeHand();
        }

        [ContextMenu("Initialise Hand")]
        public void PerformInitialization()
        {
            InitializeHand();
            foreach (var t in handBones.bones)
            {
                Debug.Log(t.Key + " " + t.Value);
            }
        }

        public void InitializeHand()
        {
            handBones = new XrHandBones(boneTransforms);
        }

        public void LerpPose(HandPose poseA, HandPose poseB, float tValue)
        {
            if (!ValidatePose(poseA) || !ValidatePose(poseB))
                return;

            foreach (string key in poseA.poseValues.Keys)
            {
                if (HandBones.bones.ContainsKey(key))
                {
                    Transform transformToChange = handBones.bones[key];
                    Quaternion rotationA = poseA.poseValues[key];
                    Quaternion rotationB = poseB.poseValues[key];

                    Quaternion lerped = Quaternion.Lerp(rotationA, rotationB, tValue);
                    transformToChange.localRotation = lerped;
                }
                else
                {
                    Debug.LogError($"Could not match (key: {key})");
                }
            }
        }


        public void UpdateHandPose(HandPose newPose)
        {
            if (!ValidatePose(newPose))
                return;
            foreach (string key in newPose.poseValues.Keys)
            {
                Transform transformToChange = HandBones.bones[key];
                Quaternion newLocalRotation = newPose.poseValues[key];
                transformToChange.localRotation = newLocalRotation;
            }
        }
        bool ValidatePose(HandPose pose)
        {
            if (pose == null)
            {
                throw new ArgumentNullException(nameof(pose));
            }
            if (pose.poseValues == null)
            {
                Debug.Log($"{pose} does not have any values");
                return false;
            }
            return true;
        }

        public HandPose BakeHandPose()
        {
            InitializeHand();
            List<string> boneNames = new List<string>();
            List<Quaternion> boneRotations = new List<Quaternion>();

            foreach (var item in handBones.bones)
            {
                boneNames.Add(item.Key);
                boneRotations.Add(item.Value.localRotation);
            }

            return new HandPose(boneRotations, boneNames);
        }

        #region Gizmos

        private void OnDrawGizmosSelected()
        {
            if (handBones != null && Application.isEditor)
            {
                foreach (var bone in handBones.bones)
                {
                    Handles.color = Color.magenta;
                    Handles.Label(bone.Value.position, bone.Key);
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (handBones != null)
            {
                foreach (Transform boneT in boneTransforms)
                {
                    Gizmos.color = Color.red;
                    if (boneT.GetChild(0) != null) Debug.DrawLine(boneT.position, boneT.GetChild(0).position);
                }
            }
        }

        #endregion
    }

    public class XrHandBones
    {
        public Dictionary<string, Transform> bones;

        public XrHandBones(List<Transform> boneTransforms)
        {
            bones = new Dictionary<string, Transform>();
            foreach (var bone in boneTransforms)
            {
                bones.Add(bone.name, bone.transform);
            }
        }
    }
}

