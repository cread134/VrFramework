using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace XrCore.Physics.Hands.Posing
{
    [System.Serializable]
    public class HandPose
    {
        public Dictionary<string, quaternion> poseValues;

        public HandPose(List<Quaternion> rotationInput, List<string> boneNames)
        {
            poseValues = new Dictionary<string, quaternion>();
            for (int i = 0; i < rotationInput.Count; i++)
            {
                poseValues.Add(boneNames[i], rotationInput[i]);
            }
        }

        public static HandPose BuildHandPose(string[] boneNames, Quaternion[] boneValues) => new HandPose(boneValues.ToList<Quaternion>(), boneNames.ToList<string>());
    }
}

