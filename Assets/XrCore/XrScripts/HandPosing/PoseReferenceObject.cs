using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseReferenceObject : MonoBehaviour
{
    public PosableHandObject rightHandPossable;
    public PosableHandObject leftHandPossable;
    public PoseObject poseToApply;
    public void ApplyPose(PoseObject pose)
    {
        rightHandPossable.InitializeHand();
        leftHandPossable.InitializeHand();
        Debug.Log(poseToApply.GetValues());
        poseToApply.CachePose();
        rightHandPossable.UpdateHandPose(pose.GetPose());
        leftHandPossable.UpdateHandPose(pose.GetPose());
    }

    [ContextMenu("Apply Pose")]
    public void ApplyActivePose()
    {
        if(poseToApply != null) ApplyPose(poseToApply);
    }
}
