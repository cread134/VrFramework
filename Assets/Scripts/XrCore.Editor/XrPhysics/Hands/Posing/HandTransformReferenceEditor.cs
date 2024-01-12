using EditorTools.Scripting;
using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

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

            var template = useTemplate ?? PrefabUtility.LoadPrefabContents(templatePath);
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
            poseRefereceObj.editorOnly = true;
        }

        public override VisualElement CreateInspectorGUI()
        {
            return GetRoot();
        }

        protected VisualElement GetRoot()
        {
            VisualElement root = new VisualElement();

            var rootObject = target as HandTransformReference;

            var dropdown = CreateDropDownSelection();
            root.Add(dropdown);

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
        #region creating enumerated types

        VisualElement CreateDropDownSelection()
        {
            var dropDown = new EnumField(HandTransformReference.GrabTypes.direct)
            {
                label = "Grab type",
                bindingPath = "grabType",
            };
            dropDown.RegisterCallback<ChangeEvent<string>>(OnEnumChange);
            return dropDown;
        }

        void OnEnumChange(ChangeEvent<string> changeEvent)
        {
            if (Enum.TryParse(changeEvent.newValue, out HandTransformReference.GrabTypes enumeration))
            {
                var rootObject = target as HandTransformReference;
                if (enumeration == rootObject.grabType) return;
                var targetType = HandTransformReference.TypeMapping[enumeration];
                if (rootObject.gameObject.GetComponent(targetType))
                {
                    return;
                }
                if (targetType == null) return;

                var newType = (HandTransformReference)rootObject.gameObject.AddComponent(targetType);
                newType.Copy(rootObject);
                newType.grabType = enumeration;

                newType.BaseValidate();
                newType.Validate();

                DestroyImmediate(rootObject);
                EditorUtility.SetDirty(newType.gameObject);
            }
        }
        #endregion

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
            targ.BaseValidate();
            targ.Validate();
            EditorUtility.SetDirty(targ);
        }
    }
}