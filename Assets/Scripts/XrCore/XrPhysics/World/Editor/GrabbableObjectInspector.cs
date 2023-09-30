using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace XrCore.XrPhysics.World.Editor
{
    [CustomEditor(typeof(GrabbableObject))]
    public class GrabbableObjectInspector : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            var grabbableObject = target as GrabbableObject;

            root.Add(new PropertyField { label = "PhysicsConfig", bindingPath = "physicsSettings"});

            return root;
        }
    }
}
