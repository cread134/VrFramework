using Core.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XrCore.XrPhysics.Hands;
using XrCore.XrPhysics.Hands.Posing;
using XrCore.XrPhysics.Interaction.Constraints;
using XrCore.XrPhysics.PhysicsObjects;
using XrCore.XrPhysics.World;

namespace XrCore.XrPhysics.Interaction
{
    public class GrabPoint : MonoBehaviour
    {
        public bool IsWithinRadius(Vector3 positionA, Vector3 positionB)
        {
            return Vector3.Distance(positionA, positionB) < maximumGrabRadius ? true : false;
        }

        [SerializeField] private float maximumGrabRadius = 0.5f;

        private void Awake()
        {
            leftHandReferenceTransforms = GetReferenceTransformValuesBySide(HandSide.Left);
            rightHandReferenceTransforms = GetReferenceTransformValuesBySide(HandSide.Right);
        }

        #region transformReferences

        [SerializeField] private HandTransformReference[] referenceTransforms;

        private HandTransformReference[] leftHandReferenceTransforms;
        public HandTransformReference[] LeftHandReferenceTransforms 
        { 
            get 
            { 
                return leftHandReferenceTransforms ??= GetReferenceTransformValuesBySide(HandSide.Left); 
            } 
        }

        private HandTransformReference[] rightHandReferenceTransforms;
        public HandTransformReference[] RightHandReferenceTransforms
        {
            get
            {
                return rightHandReferenceTransforms ??= GetReferenceTransformValuesBySide(HandSide.Right);
            }
        }

        HandTransformReference[] GetReferenceTransformValuesBySide(HandSide _side)
        {
            return referenceTransforms.Where(x => x.GetUseSide() == _side).ToArray();
        }

        public void SetReferenceTransforms(HandTransformReference[] newValues) => referenceTransforms = newValues;
        public void AddReferenceTransform(HandTransformReference newValue)
        {
            var baseList = referenceTransforms?.ToList()
                ?? new List<HandTransformReference>();
            baseList.Add(newValue);
            referenceTransforms = baseList.ToArray();
        }

        #endregion

        public bool Grabbed => isGrabbed;
        private bool isGrabbed;
        public void OnStartGrabbed()
        {
            isGrabbed = true;
        }

        public void OnRelease()
        {
            isGrabbed = false;
        }

        public void OnSpawned(GrabbableObject grabParent)
        {
            foreach (var item in referenceTransforms)
            {
                item.GrabParent = grabParent;
            }
        }

        public bool ToHandTransform(HandSide handType, Vector3 referencePosition, Vector3 forwardDirection, Vector3 upDirection, out TransformOutput possibility)
        {
            var useHands = handType == HandSide.Right ? 
                RightHandReferenceTransforms 
                : LeftHandReferenceTransforms;

            if(useHands == null || useHands.Count() == 0)
            {
                possibility = new TransformOutput(null, null);
                return false;
            }

            var values = useHands.Select(m => new TransformOutput(m.GetTransform(referencePosition, forwardDirection, upDirection), m))
                .OrderBy(x =>
                {
                    float distanceScore = 1 / Vector3.Distance(referencePosition, x.transform.position);
                    float forwardDot = Vector3.Dot(forwardDirection, x.transform.forward);
                    float UpDot = Vector3.Dot(forwardDirection, x.transform.up);
                    return distanceScore * (forwardDot + UpDot);
                })
                .Reverse();

            possibility = values.First();

            possibility = ApplyConstraints(possibility);
            return true;
        }

        #region constraints
        public IXrHandConstraint[] HandConstraints => handConstraints ?? ValidateAndInitialiseConstraints();
        private IXrHandConstraint[] handConstraints;

        public IXrHandConstraint[] ValidateAndInitialiseConstraints()
        {
            this.handConstraints = GetComponentsInChildren<IXrHandConstraint>();
            return this.handConstraints;
        }

        public (IXrHandConstraint, GameObject)[] GetHandConstraintObjects()
        {
            var constraintsObjects = GetComponentsInChildren<Transform>()
                .Where(x => x.GetComponent<IXrHandConstraint>() != null);
            var constraints = constraintsObjects
                .Select(x => (x.GetComponent<IXrHandConstraint>(), x.gameObject))
                .ToArray();
            return constraints;
        }

        TransformOutput ApplyConstraints(TransformOutput inputTransform)
        {
            var output = inputTransform;
            foreach (var item in handConstraints)
            {
                output = item.ApplyConstraint(output);
            }
            return output;
        }

        #endregion
    }
}
