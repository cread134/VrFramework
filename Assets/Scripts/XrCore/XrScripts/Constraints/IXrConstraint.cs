using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XrCore.Constraints
{
    public interface IXrConstraint
    {
        public bool ShouldApplyConstraint(Vector3 inputPosition, Quaternion inputRotation);
        public Vector3 GetConstraintPosition(Vector3 inputPosition, Quaternion inputRotation);
    }
}
