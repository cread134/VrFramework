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

        public abstract Transform GetTransform(Vector3 position, Vector3 forwardDirection, Vector3 upDirection);

        public void OnDrawGizmos()
        {
            Handles.color = Color.red;
            Gizmos.DrawWireCube(transform.position, Vector3.one * 0.01f);
            var style = new GUIStyle();
            style.fontSize = 18;
            style.alignment = TextAnchor.MiddleCenter;
            Handles.Label(transform.position, this.ToString(), style);
        }

        public override string ToString()
        {
            switch (useSide)
            {
                case HandSide.Left:
                    return "L";
                case HandSide.Right:
                    return "R";
                case HandSide.Undetermined:
                    return "U";
            }
            return "U";
        }
    }
}
