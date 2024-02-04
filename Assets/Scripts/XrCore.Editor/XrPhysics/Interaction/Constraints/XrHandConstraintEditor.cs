using System.Collections;
using System.Collections.Generic;
using Core.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace XrCore.XrPhysics.Interaction.Constraints.Editor
{
    public class XrHandConstraintEditor : VisualElement
    { 
        public XrHandConstraintEditor()
        {

        }
        public void Bind(IXrHandConstraint constraintBinding, GameObject constraintInstance)
        {
            ConstraintBinding = constraintBinding;
            ConstraintInstance = constraintInstance;
            Add(CreateConstraintMenu(constraintBinding, constraintInstance));
        }

        public VisualElement CreateConstraintMenu(IXrHandConstraint constraintBinding, GameObject constraintInstance)
        {
            var root = new VisualElement();
            root.AddSubtitle($"Instance_{ConstraintInstance.name}");
            root.AddButton("FocusInstance", () =>
            {
                Selection.activeGameObject = ConstraintInstance;
                SceneView.lastActiveSceneView.FrameSelected();
            });

            root.style.paddingTop = 10;
            root.style.marginLeft = 10;
            return root;
        }

        public void ValidateConstraint()
        {
            if (ConstraintInstance == null)
            {
                RemoveFromHierarchy();
            }
        }

        public IXrHandConstraint ConstraintBinding { get; set; }
        public GameObject ConstraintInstance { get; set; }
    }
}