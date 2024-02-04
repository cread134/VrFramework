using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XrCore.XrPhysics.Interaction.Constraints
{
    public interface IXrHandConstraint
    {
        public TransformOutput ApplyConstraint(TransformOutput inputTransform);
    }

}