using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XrCore.XrPhysics.World;

namespace XrCore.Tools.Editor
{
    public class GrabbableObjectCreator
    {
        [MenuItem("XrCore/CreateGrabbableObject")]
        public static void CreateGrabbableObject()
        {
            CreateGrabbableObjectCore();
        }

        [MenuItem("GameObject/CreateGrabbableObject")]
        public static void CreateGrabbableObjectHierachy()
        {
            CreateGrabbableObjectCore();
        }

        static GrabbableObject CreateGrabbableObjectCore()
        {
            GameObject instance = ObjectFactory.CreateGameObject("GrabbableObject", typeof(GrabbableObject));

            var grabObject = instance.GetComponent<GrabbableObject>();

            Selection.activeGameObject = instance;
            SceneView.lastActiveSceneView.FrameSelected();
            EditorUtility.SetDirty(instance);
            return grabObject;
        }
    }
}