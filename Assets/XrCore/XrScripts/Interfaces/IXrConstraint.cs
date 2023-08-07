using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IXrConstraint
{
    public bool ShouldApplyConstraint();
    public Vector3 GetConstraintPosition(GrabbableObject.StoredHandInformation handInformation, Rigidbody constraintBody);
    public Vector3 GetConstraintRotation(GrabbableObject.StoredHandInformation handInformation, Rigidbody constraintBody);
}
