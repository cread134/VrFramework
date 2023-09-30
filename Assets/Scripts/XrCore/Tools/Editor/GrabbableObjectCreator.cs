using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using XrCore.XrPhysics.World;

namespace XrCore.Tools.Editor
{
    public class GrabbableObjectCreator
    {
        [MenuItem("XrCore/CreateGrabbableObject")]
        public static void CreateGrabbableObject()
        {
            GameObject instance = ObjectFactory.CreateGameObject("GrabbableObject", typeof(GrabbableObject));
            Selection.activeGameObject = instance;
        }
    }
}