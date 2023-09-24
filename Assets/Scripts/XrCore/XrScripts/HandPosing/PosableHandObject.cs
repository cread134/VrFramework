using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PosableHandObject : MonoBehaviour
{
    private XrHandBones handBones;

    [SerializeField] private XrHand.HandSide handType;
    [SerializeField] private List<Transform> boneTransforms;


    private void Start()
    {
        if (handBones == null)
        {
            InitializeHand();
        }
    }

    [ContextMenu("Initialise Hand")]
    public void PerformInitialization()
    {
        InitializeHand();
        foreach (var t in handBones.bones)
        {
            Debug.Log(t.Key + " " + t.Value);
        }
    }

    public void InitializeHand()
    {
        handBones = new XrHandBones(boneTransforms);
    }

    private void OnDrawGizmos()
    {
        if(handBones != null)
        {
            foreach(Transform boneT in boneTransforms)
            { 
                if(boneT.GetChild(0) != null)Debug.DrawLine(boneT.position, boneT.GetChild(0).position);
            }
        }
    }

    public void LerpPose(HandPose poseA, HandPose poseB, float tValue)
    {
        try
        {
            foreach (string key in poseA.poseValues.Keys)
            {
                Transform transformToChange = handBones.bones[key];
                Quaternion rotationA = poseA.poseValues[key];
                Quaternion rotationB = poseB.poseValues[key];

                Quaternion lerped = Quaternion.Lerp(rotationA, rotationB, tValue);
                transformToChange.localRotation = lerped;
            }
        }
        catch (System.Exception)
        {
            Debug.Log("pose bone mismatch when attempting lerp");
        }
    }

    public void UpdateHandPose(HandPose newPose)
    {
        foreach (string key in newPose.poseValues.Keys)
        {
            Transform transformToChange = handBones.bones[key];
            Quaternion newLocalRotation = newPose.poseValues[key];
            transformToChange.localRotation = newLocalRotation;
        }
    }

    public HandPose BakeHandPose()
    {
        InitializeHand();
        List<string> boneNames = new List<string>();
        List<Quaternion> boneRotations = new List<Quaternion>();

        foreach (var item in handBones.bones)
        {
            boneNames.Add(item.Key);
            boneRotations.Add(item.Value.localRotation);
        }

        return new HandPose(boneRotations, boneNames);
    }
}

public class XrHandBones
{
    public Dictionary<string, Transform> bones;

    public XrHandBones(List<Transform> boneTransforms)
    {
        bones = new Dictionary<string, Transform>();
        foreach(var bone in boneTransforms)
        {
            bones.Add(bone.name, bone.transform);
        }
    }
}

