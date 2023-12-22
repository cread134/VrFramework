using Core.Extensions;
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

            var values = useHands.Select(m => m.GetTransform(referencePosition, forwardDirection, upDirection))
                .OrderBy(x =>
                {
                    float distanceScore = 1 / Vector3.Distance(referencePosition, x.position);
                    float forwardDot = Vector3.Dot(forwardDirection, x.forward);
                    float UpDot = Vector3.Dot(forwardDirection, x.up);
                    return distanceScore * (forwardDot + UpDot);
                })
                .Reverse();

#if UNITY_EDITOR
            var col = Color.red;
            var first = values.First();
            values.ForEach(x => {
                col = x == first ? Color.green : Color.red;
                Debug.DrawLine(referencePosition, x.position, col);
             });
#endif

            return values.First();
        }
    }
}
