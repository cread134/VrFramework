using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XrCore.XrPhysics.Hands.Posing
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
        { 
            if(Selection.activeGameObject == null) return false;
            return Selection.activeGameObject.TryGetComponent(out PosableHandObject go);
        }
    }
}