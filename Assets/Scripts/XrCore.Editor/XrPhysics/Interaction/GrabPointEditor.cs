using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;
using XrCore.XrPhysics.Hands.Posing;
using XrCore.XrPhysics.World;

namespace XrCore.XrPhysics.Interaction.Editor
{
    [CustomEditor(typeof(GrabPoint))]
    public class GrabPointEditor : UnityEditor.Editor
    {

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            root.Add(new PropertyField(serializedObject.FindProperty("maximumGrabRadius")));
            root.Add(new PropertyField(serializedObject.FindProperty("referenceTransforms")));

            var addButton = new Button
            {
                text = "Add new reference transform"
            };
            addButton.clicked += AddReferenceTransform;
            root.Add(addButton);

            var updateReferencesButton = new Button
            {
                text = "Update References"
            };
            updateReferencesButton.clicked += UpdateReferencesFromScript;
            root.Add(updateReferencesButton);
            return root;
        }

        #region button actions
        void AddReferenceTransform()
        {
            var rootObject = (GrabPoint)target;
            var instance = new GameObject($"{rootObject.name}_GReference{rootObject.transform.childCount + 1}");

            instance.transform.SetParent(rootObject.transform, false);
            instance.transform.localScale = Vector3.one;
            instance.transform.localPosition = Vector3.zero;

            var referenceInstance = instance.AddComponent<DirectGrabRegion>();
            rootObject.AddReferenceTransform(referenceInstance);
            Selection.activeGameObject = instance;
            SceneView.lastActiveSceneView.FrameSelected();

            UpdateReferencesFromScript();

            EditorUtility.SetDirty(rootObject);
            EditorUtility.SetDirty(instance);
        }

        void UpdateReferencesFromScript()
        {
            var rootObject = (GrabPoint)target;

            var children = rootObject.transform.GetComponentsInChildren<HandTransformReference>();
            for (int i = 0; i < children.Length; i++)
            {
                var child = children[i];
                child.transform.gameObject.name = $"{rootObject.name}_GReference{i + 1}";
                EditorUtility.SetDirty(child.gameObject);
            }
            rootObject.SetReferenceTransforms(children);
            EditorUtility.SetDirty(rootObject);
        }
        #endregion
    }
}