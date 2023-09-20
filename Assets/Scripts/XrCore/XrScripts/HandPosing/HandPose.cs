using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

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
}

