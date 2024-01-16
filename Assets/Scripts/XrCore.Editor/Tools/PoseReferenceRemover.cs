using UnityEditor;
using UnityEngine;
using XrCore.XrPhysics.Hands.Posing;

namespace XrCore.Tools
{
    public class PoseReferenceRemover
    {
        [MenuItem("XrCore/PoseReferenceRemover")]
        public static void RemovePoseReferences()
        {
            var poseReferences = GameObject.FindObjectsOfType<PoseReferenceObject>();
            foreach(var poseReference in poseReferences)
            {
                Object.DestroyImmediate(poseReference);
            }
        }

        [MenuItem("XrCore/ShowHidden")]
        public static void ShowHidden()
        {
            var poseReferences = GameObject.FindObjectsOfType<Transform>();
            foreach (var poseReference in poseReferences)
            {
                poseReference.gameObject.hideFlags = HideFlags.None;
            }
        }
    }
}