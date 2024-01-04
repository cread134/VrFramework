using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XrCore.XrPhysics.Hands.Posing
{
    [CreateAssetMenu]
    public class PoseMask : ScriptableObject
    {
        public List<HandMaskEnum> registeredMasks;
        public PoseMask(params HandMaskEnum[] masks) 
        {
            registeredMasks = new List<HandMaskEnum>();
            foreach (var mask in masks)
            {
                AddMask(mask);
            }
        }

        public void AddMask(HandMaskEnum mask)
        {
            if(!registeredMasks.Contains(mask))
            {
                registeredMasks.Add(mask);
            }
        }

        public void RemoveMask(HandMaskEnum mask)
        {
            if(registeredMasks.Contains(mask))
            {
                registeredMasks.Remove(mask);
            }
        }
    }
    public enum HandMaskEnum
    {
        Pinky,
        Index,
        Ring,
        Thumb,
        Middle,
    }
}