using Core.Extensions;
using System;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UIElements;
using XrCore.Context;
using XrCore.Interaction.Control;
using XrCore.XrPhysics.Hands;
using static UnityEngine.GraphicsBuffer;

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

            var controller = context.GetController(handSide);
            var controlsOpen = rightOpen;
            Action accessAction = AccessRightHand;
            Action mainButtonAction = OnRightMainButton;
            Action secondaryButtonAction = OnRightSecondaryButton;
            EventCallback<ChangeEvent<float>> gripAction = OnRightGripChange;
            EventCallback<ChangeEvent<float>> triggerAction = OnRightTriggerChange;
            if (handSide == HandSide.Left)
            {
                controlsOpen = leftOpen;
                accessAction = AccessLeftHand;
                mainButtonAction = OnLeftMainButton;
                secondaryButtonAction = OnLeftSecondaryButton;
                gripAction = OnLeftGripChange;
                triggerAction = OnLeftTriggerChange;
            }

            var horizontalBox = new VisualElement();
            horizontalBox.style.alignContent = Align.FlexStart;
            horizontalBox.style.flexDirection = FlexDirection.Row;
            horizontalBox.AddButton($"{handSide}Hand", accessAction);
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
                root.style.width = 300;
                controlsButton.style.backgroundColor = Color.grey;
                root.AddSubtitle($"Controls {handSide}");
                root.AddSlider("Grip", 1f, 0f, gripAction, controller.ReadGrip());
                root.AddSlider("Trigger", 1f, 0f, triggerAction, controller.ReadTrigger());
                root.AddButton("MainButton", mainButtonAction);
                root.AddButton("SecondaryButton", secondaryButtonAction);
            }

            return root;
        }

        #region updateGrip
        void OnRightGripChange(ChangeEvent<float> changeEvent) => OnGripSliderChange(changeEvent, HandSide.Right);
        void OnLeftGripChange(ChangeEvent<float> changeEvent) => OnGripSliderChange(changeEvent, HandSide.Left);
        void OnGripSliderChange(ChangeEvent<float> changeEvent, HandSide side)
        {
            var context = GameObject.FindFirstObjectByType<XrContext>();
            if (context == null) return;

            var element = (Slider)changeEvent.target;
            element.label = $"Grip {changeEvent.newValue}";

            var handController = context.GetController(side);
            handController.UpdateGrip(changeEvent.newValue);
        }
        #endregion

        #region updateTrigger

        void OnLeftTriggerChange(ChangeEvent<float> changeEvent) => OnTriggerSliderChange(changeEvent, HandSide.Right);
        void OnRightTriggerChange(ChangeEvent<float> changeEvent) => OnTriggerSliderChange(changeEvent, HandSide.Left);
        void OnTriggerSliderChange(ChangeEvent<float> changeEvent, HandSide side)
        {
            var context = GameObject.FindFirstObjectByType<XrContext>();
            if (context == null) return;

            var element = (Slider)changeEvent.target;
            element.label = $"Trigger {changeEvent.newValue}";

            var handController = context.GetController(side);
            handController.UpdateTrigger(changeEvent.newValue);
        }
        #endregion

        #region mainButton
        void OnRightMainButton() => OnMainButtonPressed(HandSide.Right);
        void OnLeftMainButton() => OnMainButtonPressed(HandSide.Left);
        void OnMainButtonPressed(HandSide handSide)
        {
            var context = GameObject.FindFirstObjectByType<XrContext>();
            if (context == null) return;
            var controller = context.GetController(handSide);

            controller.OnMainButtonDown();
        }
        #endregion

        #region secondaryButton
        void OnRightSecondaryButton() => OnSecondaryButtonPressed(HandSide.Right);
        void OnLeftSecondaryButton() => OnSecondaryButtonPressed(HandSide.Left);
        void OnSecondaryButtonPressed(HandSide handSide)
        {
            var context = GameObject.FindFirstObjectByType<XrContext>();
            if (context == null) return;
            var controller = context.GetController(handSide);

            controller.OnSecondaryButtonDown();
        }
        #endregion

        void Repaint()
        {
            //this fucking sucks, but its the only way to repaint :(
            collapsed = true;
            collapsed = false;
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
