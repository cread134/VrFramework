using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XrCore.XrPhysics.Hands.Posing
{
    public class HandTransformReference : MonoBehaviour
    {
        [SerializeField] private PoseObject targetPose;

        public HandPose GetTargetPose()
        {
            return targetPose.HandPose;
        }
    }
}
