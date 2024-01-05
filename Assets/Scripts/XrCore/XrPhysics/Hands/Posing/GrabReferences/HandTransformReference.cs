using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XrCore.XrPhysics.PhysicsObjects;

namespace XrCore.XrPhysics.Hands.Posing
{
    [SelectionBase]
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
            var style = new GUIStyle();
            style.fontSize = 18;
            style.alignment = TextAnchor.MiddleCenter;
            Gizmos.DrawIcon(transform.position, "empty.png", true);
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
