using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XrCore.XrPhysics.Hands.Posing
{
    public class HandTransformReference : MonoBehaviour
    {
        [SerializeField] private HandSide useSide;
        [SerializeField] private PoseObject targetPose;

        public HandPose GetTargetPose() => targetPose.HandPose;
        public HandSide GetUseSide() => useSide;
    }
}
