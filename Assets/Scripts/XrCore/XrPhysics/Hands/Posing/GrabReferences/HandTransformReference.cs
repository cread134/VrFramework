using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XrCore.XrPhysics.PhysicsObjects;

namespace XrCore.XrPhysics.Hands.Posing
{
    public abstract class HandTransformReference : MonoBehaviour
    {
        [SerializeField] private HandSide useSide;
        [SerializeField] private PoseObject targetPose;

        public HandPose GetTargetPose() => targetPose?.HandPose;
        public HandSide GetUseSide() => useSide;

        public abstract SimpleTransform GetTransform(SimpleTransform reference);


        private void OnDrawGizmos()
        {
            Gizmos.DrawIcon(transform.position, "HandIcon.png", false, useSide == HandSide.Right ? Color.red : Color.blue);
        }
    }
}
