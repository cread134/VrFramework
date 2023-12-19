using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XrCore.XrPhysics.PhysicsObjects;

namespace XrCore.XrPhysics.Hands.Posing
{
    public class CircleGrabRegion : HandTransformReference
    {
        public override Transform GetTransform(Vector3 position, Vector3 forwardDirection, Vector3 upDirection)
        {
            return transform;
        }
    }
}
