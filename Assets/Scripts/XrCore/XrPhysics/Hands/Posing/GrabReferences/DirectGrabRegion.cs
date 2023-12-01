using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XrCore.XrPhysics.PhysicsObjects;

namespace XrCore.XrPhysics.Hands.Posing
{
    public class DirectGrabRegion : HandTransformReference
    {
        public override SimpleTransform GetTransform(SimpleTransform reference)
        {
            return new SimpleTransform(transform.up, transform.forward, transform.position);
        }
    }
}
