using System.Collections;
using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using XrCore.XrPhysics.World;
using ScriptingResources;
using Core.Extensions;
using XrCore.XrPhysics.Hands.Posing;
using XrCore.Context;

namespace XrCore.Tools
{
    [Overlay(typeof(SceneView), "GrabPointControls", true)]
    public class GrabPointControlOverlay : Overlay
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

            var goTarget = Selection.activeGameObject?.GetComponent<HandTransformReference>();
            if (goTarget != null)
            {
                root.AddHeader("Point controls");
                root.AddButton("Grab point", () => SelectGrabPoint(goTarget));
            }
            return root;
        }

        void SelectGrabPoint(HandTransformReference goTarget)
        {
            if(!Application.isPlaying) return;
            var context = GameObject.FindFirstObjectByType<XrContext>();
            if (context != null)
            {
                var handtype = goTarget.GetUseSide();
                var useHand = context.GetHand(handtype);
                var conroller = context.GetController(handtype);
                useHand.GrabFromPoint(goTarget);

                Selection.activeGameObject = conroller.gameObject;
                SceneView.lastActiveSceneView.FrameSelected();
            }
        }

        bool validSelection;
        void OnObjectSelected()
        {
            var selectedGameObject = Selection.activeGameObject;
            if(selectedGameObject != null && selectedGameObject.TryGetComponent(out HandTransformReference go))
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
