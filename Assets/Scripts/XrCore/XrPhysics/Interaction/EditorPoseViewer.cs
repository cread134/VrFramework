using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XrCore.XrPhysics.Hands;
using XrCore.XrPhysics.Hands.Posing;

namespace XrCore.XrPhysics.Interaction.Editor
{
    public class EditorPoseViewer : MonoBehaviour
    {
        public void SetHandPose(HandPose pose)
        {
            var handObjects = GetComponentsInChildren<PosableHandObject>();
            foreach (var handObject in handObjects)
            {
                handObject.UpdateHandPose(pose);
            }
        }

        public void SetHandSide(HandSide handSide)
        {
            var handObjects = GetComponentsInChildren<PosableHandObject>();
            foreach (var handObject in handObjects)
            {
                if(handObject.handType != handSide)
                {
                    handObject.gameObject.SetActive(false);
                }
            }
        }
    }
}