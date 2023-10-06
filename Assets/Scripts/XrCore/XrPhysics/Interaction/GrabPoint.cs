using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XrCore.XrPhysics.Hands;

namespace XrCore.XrPhysics.Interaction
{
    public class GrabPoint : MonoBehaviour
    {
        public bool IsWithinRadius(Vector3 positionA, Vector3 positionB)
        {
            return Vector3.Distance(positionA, positionB) < maximumGrabRadius ? true : false;
        }

        [Header("Grab settings")]
        [SerializeField] private float maximumGrabRadius = 0.5f;
        [SerializeField] private Color leftHandColor;
        [SerializeField] private Color rightHandColor;

        [SerializeField] private Transform[] leftHandReferenceTransforms;
        [SerializeField] private Transform[] rightHandReferenceTransforms;

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

        private void OnDrawGizmos()
        {
            if (leftHandReferenceTransforms != null && rightHandReferenceTransforms != null)
            {
                foreach (var item in leftHandReferenceTransforms)
                {
                    Gizmos.color = leftHandColor;
                    Gizmos.DrawSphere(item.transform.position, 0.01f);
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(item.position, item.position + (item.forward * 0.03f));
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(item.position, item.position + (item.up * 0.03f));
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(item.position, item.position + (item.right * 0.03f));
                }

                foreach (var item in rightHandReferenceTransforms)
                {
                    Gizmos.color = rightHandColor;
                    Gizmos.DrawSphere(item.transform.position, 0.01f);
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(item.position, item.position + (item.forward * 0.03f));
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(item.position, item.position + (item.up * 0.03f));
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(item.position, item.position + (item.right * 0.03f));
                }
            }
        }
    }
}
