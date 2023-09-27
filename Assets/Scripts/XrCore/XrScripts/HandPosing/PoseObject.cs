using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace XrCore.Physics.Hands.Posing
{
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
        public HandPose HandPose
        {
            get
            {
                if (_instancedPose == null || _instancedPose.poseValues == null)
                {
                    var value = HandPose.BuildHandPose(boneNames, boneValues);
                    _instancedPose = value;
                    return value;
                }
                return _instancedPose;
            }
        }
    }
}
