using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XrCore.Constraints
{
    public class XrLinearConstraint : MonoBehaviour, IXrConstraint
    {
        public Vector3 GetConstraintPosition(Vector3 inputPosition, Quaternion inputRotation)
        {
            throw new System.NotImplementedException();
        }

        public bool ShouldApplyConstraint(Vector3 inputPosition, Quaternion inputRotation)
        {
            throw new System.NotImplementedException();
        }
    }
}

