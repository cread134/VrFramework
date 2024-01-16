using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XrCore.XrPhysics.PhysicsObjects;

namespace XrCore.XrPhysics.Hands.Posing
{
    public class LinearGrabRegion : HandTransformReference
    {
        public Transform start;
        public Transform end;
        public Transform middle;

        public override Transform GetTransform(Vector3 position, Vector3 forwardDirection, Vector3 upDirection)
        {
            var projectedVector = Vector3.Project(position - start.position, end.position - start.position);
            var worldSpaceVector = start.position + projectedVector;
            middle.transform.rotation = transform.rotation;
            middle.transform.position = worldSpaceVector;
            return middle;
        }

        public override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            if (start != null && end != null) { 
                Gizmos.DrawLine(start.position, end.position);
            }
        }
    }
}
