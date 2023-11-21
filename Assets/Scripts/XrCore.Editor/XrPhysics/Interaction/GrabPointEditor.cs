using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using XrCore.XrPhysics.World;

namespace XrCore.XrPhysics.Interaction.Editor
{
    [CustomEditor(typeof(GrabPoint))]
    public class GrabPointEditor : UnityEditor.Editor
    {
        GameObject[] previewInstances;

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            root.Add(new PropertyField(serializedObject.FindProperty("maximumGrabRadius")));
            root.Add(new PropertyField(serializedObject.FindProperty("leftHandColor")));
            root.Add(new PropertyField(serializedObject.FindProperty("rightHandColor")));
            root.Add(new PropertyField(serializedObject.FindProperty("referenceTransforms")));

            var addButton = new Button
            {
                text = "Add new reference transform"
            };
            addButton.clicked += AddReferenceTransform;
            root.Add(addButton);
          
            return root;
        }

        void AddReferenceTransform()
        {

        }

        private void OnDestroy()
        {
            if (previewInstances != null)
            {
                foreach (GameObject go in previewInstances)
                {
                    DestroyImmediate(go);
                }
            }
        }
    }
}