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
        bool leftOpen;
        bool rightOpen;

        public override VisualElement CreatePanelContent()
        {

            var root = new VisualElement() { name = "Xr Toolbar" };
            var context = GameObject.FindFirstObjectByType<XrContext>();
            if(context == null )
            {
                root.AddHeader("No XrContext found");
                return root;
            }

            root.AddHeader("XR TOOLS");

            root.Add(CreateHandElement(HandSide.Right, context));
            root.Add(CreateHandElement(HandSide.Left, context));
            return root;

        }

        VisualElement CreateHandElement(HandSide handSide, XrContext context)
        {
            var root = new VisualElement();
            var hand = context?.GetController(handSide);
            var controlsOpen = handSide == HandSide.Right ? rightOpen : leftOpen;

            var horizontalBox = new VisualElement();
            horizontalBox.style.alignContent = Align.FlexStart;
            horizontalBox.style.flexDirection = FlexDirection.Row;
            horizontalBox.AddButton($"{handSide}Hand", AccessRightHand);
            var controlsButton = horizontalBox.AddButton("Ctrl", () =>
            {
                if (handSide == HandSide.Right)
                {
                    rightOpen = !rightOpen;
                }
                else
                {
                    leftOpen = !leftOpen;
                }
                Repaint();
            });

            root.Add(horizontalBox);
            if (controlsOpen)
            {
                controlsButton.style.backgroundColor = Color.grey;
                root.AddSubtitle($"Controls {handSide}");
            }

            return root;
        }

        void Repaint()
        {
            //this fucking sucks, but its the only way to repaint :(
            collapsed = true;
            collapsed = false;
        }

        VisualElement CreateControlsElement(IXrHandControls controls)
        {
            return new VisualElement() { name = "ControlsRight" };
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
