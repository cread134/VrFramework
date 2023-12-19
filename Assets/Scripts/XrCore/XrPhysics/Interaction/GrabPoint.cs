using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XrCore.XrPhysics.Hands;
using XrCore.XrPhysics.Hands.Posing;
using XrCore.XrPhysics.PhysicsObjects;

namespace XrCore.XrPhysics.Interaction
{
    public class GrabPoint : MonoBehaviour
    {
        public bool IsWithinRadius(Vector3 positionA, Vector3 positionB)
        {
            return Vector3.Distance(positionA, positionB) < maximumGrabRadius ? true : false;
        }

        [SerializeField] private float maximumGrabRadius = 0.5f;

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
            List<HandTransformReference> values = new List<HandTransformReference>();
            foreach (var item in referenceTransforms)
            {
                if (item.GetUseSide() == _side)
                {
                    values.Add(item);
                } 
            }
            return values.ToArray();
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

        public Transform ToHandTransform(HandSide handType, Vector3 referencePosition, Vector3 forwardDirection, Vector3 upDirection)
        {
            var useHands = handType == HandSide.Right ? 
                RightHandReferenceTransforms 
                : LeftHandReferenceTransforms;

            (int index, float score) matchingValues = (0, 0f);
            for (int i = 0; i < useHands.Length; i++)
            {
                var mactchingVal = useHands[i].GetTransform(referencePosition, forwardDirection, upDirection);

                float distanceScore = 1 / Vector3.Distance(referencePosition, mactchingVal.position);
                float forwardDot = Vector3.Dot(forwardDirection, mactchingVal.forward);
                float UpDot = Vector3.Dot(forwardDirection, mactchingVal.up);
                float attributedScore = distanceScore * (forwardDot + UpDot);
                if (attributedScore > matchingValues.score)
                {
                    matchingValues.score = attributedScore;
                    matchingValues.index = i;
                }
            }

            return useHands[matchingValues.index].transform;
        }
    }
}
