using UnityEngine;
using XrCore.XrPhysics.Hands.Posing;

namespace XrCore.XrPhysics.Interaction
{
    public struct TransformOutput
    {
        public Transform transform;
        public HandTransformReference referenceTransform;

        public TransformOutput(Transform transform, HandTransformReference referenceTransform)
        {
            this.transform = transform;
            this.referenceTransform = referenceTransform;
        }

    }
}