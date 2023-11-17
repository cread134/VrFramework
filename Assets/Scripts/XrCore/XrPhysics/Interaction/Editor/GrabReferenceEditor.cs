using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using XrCore.XrPhysics.Hands.Posing;

namespace XrCore.XrPhysics.Interaction.Editor
{
    [CustomEditor(typeof(HandTransformReference))]
    public class GrabReferenceEditor : UnityEditor.Editor
    {
        private GameObject previewInstance;

        public void OnEnable()
        {
            
        }

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            var sourceObj = target as GameObject;

            return root;
        }

        public void OnDestroy()
        {
            Debug.Log("destroyeditr");
            if (previewInstance != null)
            {
                DestroyImmediate(previewInstance);
            }
        }
    }
}