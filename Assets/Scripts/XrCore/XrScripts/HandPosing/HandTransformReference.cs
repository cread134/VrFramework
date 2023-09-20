using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTransformReference : MonoBehaviour
{
    [SerializeField] private PoseObject targetPose;
    private void Awake()
    {
        targetPose.CachePose();
    }

    public HandPose GetTargetPose()
    {
        return targetPose.GetPose();
    }
}
