using UnityEngine;

namespace XrCore.XrPhysics.Interaction.Constraints
{
    public class LinearXrConstraint : MonoBehaviour, IXrHandConstraint
    {
        public Transform startPosition;
        public Transform endPosition;

        public TransformOutput ApplyConstraint(TransformOutput inputTransform)
        {
            throw new System.NotImplementedException();
        }

        private void OnDrawGizmosSelected()
        {
            
        }
    }
}