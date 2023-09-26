using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class PoseObject : ScriptableObject
{
    public string _poseName;
    public string[] boneNames;
    public Quaternion[] boneValues;

    public string GetValues()
    {
        string toReturn = "";
        for (int i = 0; i < boneNames.Length; i++)
        {
            toReturn += boneNames[i] + ": " + boneValues[i].ToString();
        }
        return toReturn;
    }

    private HandPose _instancedPose;
    public HandPose GetPose()
    {
        if( _instancedPose == null)
            return CachePose();

        return _instancedPose; 
    }

    public HandPose CachePose()
    {
        var pose = new HandPose(boneValues.ToList<Quaternion>(), boneNames.ToList<string>());
        _instancedPose = pose;
        return pose;
    }
}
