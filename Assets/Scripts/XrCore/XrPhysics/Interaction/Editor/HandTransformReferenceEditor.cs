using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using XrCore.XrPhysics.Hands.Posing;

namespace XrCore.XrPhysics.Interaction.Editor
{
    [CustomEditor(typeof(HandTransformReference))]
    public class HandTransformReferenceEditor : UnityEditor.Editor
    {
        public GameObject useTemplate;
        const string templatePath = "Assets/Prefabs/XrCore/PoseReference.prefab";
        private GameObject previewInstance;

        public void OnEnable()
        {
            DestroyPreviewInstance();
            var template = useTemplate ?? AssetDatabase.LoadAssetAtPath(templatePath, typeof(PoseReferenceObject)) as GameObject;
            if (template == null)
            {
                Debug.Log("no hand template found");
                return;
            }

            previewInstance = Instantiate(template, (target as HandTransformReference).transform);
            previewInstance.hideFlags = HideFlags.HideInHierarchy;

            UpdatePreviewState();
        }

        void UpdatePreviewState()
        {
            if (previewInstance == null) return;
            var baseObject = target as HandTransformReference;
            var targetSide = baseObject.GetUseSide();
            var poseRefereceObj = previewInstance.GetComponent<PoseReferenceObject>();
            poseRefereceObj.SetBoneVisibility(false, targetSide);
        }

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            root.Add(new PropertyField(serializedObject.FindProperty("targetPose")));
            root.Add(new PropertyField(serializedObject.FindProperty("useSide")));

            return root;
        }

        public void OnDisable() => DestroyPreviewInstance();
        public void OnDestroy() => DestroyPreviewInstance();

        void DestroyPreviewInstance()
        {
            if (previewInstance != null)
            {
                DestroyImmediate(previewInstance);
            }
        }
    }
}