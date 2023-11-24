using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using XrCore.Scripting;
using XrCore.XrPhysics.Hands.Posing;

namespace XrCore.XrPhysics.Interaction.Editor
{
    [CustomEditor(typeof(HandTransformReference))]
    public class HandTransformReferenceEditor : UnityEditor.Editor
    {
        const string templatePath = "Assets/Prefabs/XrCore/PoseReference.prefab";
        static Color RightHandColor = new Color(1f, 0f, 0f, 0.3f);
        static Color LeftHandColor = new Color(0f, 0f, 1f, 0.3f);

        public GameObject useTemplate;
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
            poseRefereceObj.SetBoneVisibility(false);
            poseRefereceObj.SetHandVisibility(true, targetSide);
            poseRefereceObj.SetHandTransform(baseObject.transform.position, baseObject.transform.rotation, targetSide);
        }

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            var poseField = new PropertyField(serializedObject.FindProperty("targetPose"));
            poseField.RegisterValueChangeCallback(x => OnPoseChanged(x.changedProperty));
            root.Add(poseField);
            root.Add(new PropertyField(serializedObject.FindProperty("useSide")));

            return root;
        }

        void OnPoseChanged(SerializedProperty newValue)
        {
            var value = newValue.GetValue<PoseObject>();
            var baseObject = target as HandTransformReference;

            if (value != null && previewInstance != null)
            {
                var poseReference = previewInstance.GetComponent<PoseReferenceObject>();
                poseReference.SetHandPose(value.HandPose, baseObject.GetUseSide());
            }
        }

        public void OnDisable() => DestroyPreviewInstance();
        public void OnDestroy() => DestroyPreviewInstance();

        void DestroyPreviewInstance()
        {
            if (previewInstance != null && previewInstance != null)
            {
                DestroyImmediate(previewInstance);
            }
        }
    }
}