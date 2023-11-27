using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XrCore.XrPhysics.Structs;

namespace XrCore.XrPhysics.Hands.Posing
{
    public class HandTransformReference : MonoBehaviour
    {
        [SerializeField] private HandSide useSide;
        [SerializeField] private PoseObject targetPose;

        public HandPose GetTargetPose() => targetPose.HandPose;
        public HandSide GetUseSide() => useSide;

        public SimpleTransform GetTransform(SimpleTransform reference)
        {
            return new SimpleTransform(transform.up, transform.forward, transform.position);
        }


        private void OnDrawGizmos()
        {
            Gizmos.DrawIcon(transform.position, "HandIcon.png", false, useSide == HandSide.Right ? Color.red : Color.blue);
        }
    }
}
