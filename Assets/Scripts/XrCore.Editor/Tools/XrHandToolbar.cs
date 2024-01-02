using Core.Extensions;
using System;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UIElements;
using XrCore.Context;
using XrCore.Interaction.Control;
using XrCore.XrPhysics.Hands;

namespace XrCore.Tools
{
    [Overlay(typeof(SceneView), "Xr Controls", true)]
    public class XrHandToolbar : Overlay
    {
        public override void OnCreated()
        {
            Selection.selectionChanged += OnSelectedChanged;
        }

        public override void OnWillBeDestroyed()
        {
            Selection.selectionChanged -= OnSelectedChanged;
        }

        public override VisualElement CreatePanelContent()
        {

            var root = new VisualElement() { name = "Xr Toolbar" };
            root.AddHeader("XR TOOLS");

            root.AddButton("RightHand", AccessRightHand);
            root.AddButton("LeftHand", AccessLeftHand);

            if (Selection.activeGameObject != null && Selection.activeGameObject.TryGetComponent<HandController>(out var controller))
            {
                root.AddHeader("Controls");
                
            }
            return root;

        }

        void OnSelectedChanged()
        {
            if(Selection.activeGameObject != null)
            {
                CreatePanelContent();
            }
        }

        void AccessRightHand() => AccessHandCore(HandSide.Right);
        void AccessLeftHand() => AccessHandCore(HandSide.Left);

        void AccessHandCore(HandSide handSide)
        {
            var context = GameObject.FindFirstObjectByType<XrContext>();
            var hand = context?.GetController(handSide);
            if (hand != null)
            {
                Selection.activeTransform = hand.transform;
                SceneView.lastActiveSceneView.FrameSelected();
            }

            CreatePanelContent();
        }
    }
}
