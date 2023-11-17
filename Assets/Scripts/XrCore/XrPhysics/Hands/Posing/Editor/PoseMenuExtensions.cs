using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XrCore.XrPhysics.Hands.Posing.Editor
{
    public class PoseMenuExtensions 
    {
        [MenuItem("GameObject/Pose/OpenPoseMenu", priority = 1)]
        private static void OpenPoseMenu()
        {
            PoseEditor poseEditor = EditorWindow.GetWindow<PoseEditor>();
            poseEditor.Show();
        }

        [MenuItem("GameObject/Pose/OpenPoseMenu", validate = true)]
        private static bool PoseMenuValidation() 
            => Selection.activeGameObject.TryGetComponent(out PosableHandObject go);
    }
}