using UnityEditor;
using UnityEngine;
using XrCore.XrPhysics.World;

namespace XrCore.XrPhysics.Hands.Posing
{
    [SelectionBase]
    public abstract class HandTransformReference : MonoBehaviour
    {
        [SerializeField] private HandSide useSide;
        [SerializeField] private PoseObject targetPose;

        public GrabbableObject GrabParent 
        {
            get 
            {
               return grabParent ??= GetGrabParent();
            }
            set { grabParent = value; }
        }
        public GrabbableObject grabParent;

        GrabbableObject GetGrabParent()
        {
            Transform cur = transform;
            while(cur.parent != null)
            {
                cur = cur.parent;
                if(cur == null ) break;
                if(cur.gameObject.TryGetComponent(out GrabbableObject grabbableObject))
                {
                    return grabbableObject;
                }
            }
            return null;
        }

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
