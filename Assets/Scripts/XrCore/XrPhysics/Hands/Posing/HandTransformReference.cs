using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XrCore.XrPhysics.Hands.Posing
{
    public class HandTransformReference : MonoBehaviour
    {
        [SerializeField] private HandSide useSide;
        [SerializeField] private PoseObject targetPose;

        public HandPose GetTargetPose() => targetPose.HandPose;
        public HandSide GetUseSide() => useSide;

        private void OnDrawGizmos()
        {
            Gizmos.DrawIcon(transform.position, "HandIcon.png", false, useSide == HandSide.Right ? Color.red : Color.blue);
        }
    }
}
