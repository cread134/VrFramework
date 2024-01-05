using System.Collections;
using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using XrCore.XrPhysics.World;
using ScriptingResources;
using Core.Extensions;

namespace XrCore.Tools
{
    [Overlay(typeof(SceneView), "ObjectControls", true)]
    public class GrabbableObjectToolbar : Overlay
    {
        public override void OnCreated()
        {
            Selection.selectionChanged += OnObjectSelected;
        }

        public override void OnWillBeDestroyed()
        {
            Selection.selectionChanged -= OnObjectSelected;
        }

        public override VisualElement CreatePanelContent()
        {
            VisualElement root = new VisualElement { name = "ObjectControls" };

            var goTarget = Selection.activeGameObject?.GetComponent<GrabbableObject>();
            if (goTarget != null)
            {
                root.AddHeader("ObjectControls");
            }
            return root;
        }

        bool validSelection;
        void OnObjectSelected()
        {
            var selectedGameObject = Selection.activeGameObject;
            if(selectedGameObject != null && selectedGameObject.TryGetComponent(out GrabbableObject go))
            {
                if(validSelection == false)
                {
                    this.Redraw();
                }
                validSelection = true;
            } else
            {
                if(validSelection == true)
                {
                    this.Redraw();
                }
                validSelection = false;
            }
        }
    }
}
