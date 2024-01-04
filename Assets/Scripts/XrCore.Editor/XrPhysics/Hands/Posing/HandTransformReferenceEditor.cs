using EditorTools.Scripting;
using ScriptingResources.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using XrCore.XrPhysics.Hands;
using XrCore.XrPhysics.Hands.Posing;

namespace XrCore.XrPhysics.Hands.Posing
{
    [CustomEditor(typeof(HandTransformReference))]
    public class HandTransformReferenceEditor : UnityEditor.Editor
    {
        const string templatePath = "Assets/Prefabs/XrCore/PoseReference.prefab";

        public GameObject useTemplate;
        private GameObject previewInstance;
        
        public void OnEnable()
        {
            DestroyPreviewInstance();
            ValidateObject();

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
            return GetRoot();
        }

        protected VisualElement GetRoot()
        {
            VisualElement root = new VisualElement();

            var rootObject = target as HandTransformReference;

            var poseField = new PropertyField(serializedObject.FindProperty("targetPose"));
            poseField.RegisterValueChangeCallback(x => OnPoseChanged(x.changedProperty));
            root.Add(poseField);

            var useSideField = new PropertyField(serializedObject.FindProperty("useSide"));
            useSideField.RegisterValueChangeCallback(x =>
            {
                UpdatePreviewState();
                var pose = rootObject.GetTargetPose();
                if (pose != null)
                {
                    UpdateHandPose(rootObject.GetTargetPose());
                }
            });
            root.Add(useSideField);

            return root;
        }

        void OnPoseChanged(SerializedProperty newValue)
        {
            var value = newValue.GetValue<PoseObject>();
            var baseObject = target as HandTransformReference;

            if (value != null && previewInstance != null)
            {
                UpdateHandPose(value.HandPose);
            }
        }

        void UpdateHandPose(HandPose pose)
        {
            var baseObject = target as HandTransformReference;
            var poseReference = previewInstance.GetComponent<PoseReferenceObject>();
            poseReference.SetHandPose(pose, baseObject.GetUseSide());

            EditorUtility.SetDirty(baseObject);
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

        void ValidateObject()
        {
            var targ = target as HandTransformReference;
            var childCount = targ.transform.childCount;
            var children = targ.transform.GetComponentsInChildren<Transform>()
                .Where(x => x.transform != targ.transform)
                .ToArray();
            for (int i = 0; i < childCount; i++)
            {
                DestroyImmediate(children[i].gameObject);
            }
            if(childCount > 0)
            {
                EditorUtility.SetDirty(targ);
            }
        }
    }
}