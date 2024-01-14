using UnityEngine;

namespace XrCore.XrPhysics.Hands.Posing
{
    public class DirectGrabRegion : HandTransformReference
    {
        public override Transform GetTransform(Vector3 position, Vector3 forwardDirection, Vector3 upDirection)
        {
            return transform;
        }
    }
}
