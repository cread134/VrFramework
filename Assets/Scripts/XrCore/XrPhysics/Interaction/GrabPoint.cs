using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XrCore.XrPhysics.Hands;
using XrCore.XrPhysics.Hands.Posing;

namespace XrCore.XrPhysics.Interaction
{
    public class GrabPoint : MonoBehaviour
    {
        public bool IsWithinRadius(Vector3 positionA, Vector3 positionB)
        {
            return Vector3.Distance(positionA, positionB) < maximumGrabRadius ? true : false;
        }

        [SerializeField] private float maximumGrabRadius = 0.5f;

        [SerializeField] private HandTransformReference[] referenceTransforms;

        public Transform[] leftHandReferenceTransforms;
        public Transform[] rightHandReferenceTransforms;

        public void SetReferenceTransforms(HandTransformReference[] newValues) => referenceTransforms = newValues;
        public void AddReferenceTransform(HandTransformReference newValue)
        {
            var baseList = referenceTransforms.ToList();
            baseList.Add(newValue);
            referenceTransforms = baseList.ToArray();
        }

        private bool isGrabbed;
        public bool Grabbed() { return isGrabbed; }

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
            Transform[] useHands = leftHandReferenceTransforms;
            if (handType == HandSide.Right) useHands = rightHandReferenceTransforms;


            (int index, float score) matchingValues = (0, 0f);
            for (int i = 0; i < useHands.Length; i++)
            {
                float distanceScore = 1 / Vector3.Distance(referencePosition, useHands[i].position);
                float forwardDot = Vector3.Dot(forwardDirection, useHands[i].forward);
                float UpDot = Vector3.Dot(forwardDirection, useHands[i].up);
                float attributedScore = distanceScore * (forwardDot + UpDot);
                if (attributedScore > matchingValues.score)
                {
                    matchingValues.score = attributedScore;
                    matchingValues.index = i;
                }
            }

            return useHands[matchingValues.index];
        }
    }
}
