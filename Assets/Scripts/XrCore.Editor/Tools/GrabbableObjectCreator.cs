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
            instance.layer = 7;


            var grabObject = instance.GetComponent<GrabbableObject>();
            var boxCollider = instance.AddComponent<BoxCollider>();
            boxCollider.size = Vector3.one * 0.1f;

            Selection.activeGameObject = instance;
            SceneView.lastActiveSceneView.FrameSelected();
            EditorUtility.SetDirty(instance);
            return grabObject;
        }
    }
}