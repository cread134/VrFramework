using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UIElements;
using Core.Extensions;
using XrCore.Interaction.Control;

namespace XrCore.Interaction.Editor
{

    [CustomEditor(typeof(HandController))]
    public class XrControllerEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            root.Add(new PropertyField { label = "XrHand", bindingPath = "xrHand" });
            root.Add(new PropertyField(serializedObject.FindProperty("Controller")));

            root.AddHeader("Input Values");
            root.Add(new PropertyField(serializedObject.FindProperty("MainButtonAction")));
            root.Add(new PropertyField(serializedObject.FindProperty("SecondaryButtonAction")));

            var gripSlider = new Slider
            {
                bindingPath = "gripValue",
                highValue = 1f,
                lowValue = 0f,
                showMixedValue = true,
            };
            gripSlider.label = $"GripValue {gripSlider.value}";
            gripSlider.RegisterCallback<ChangeEvent<float>>(OnGripSliderChange);

            var triggerSlider = new Slider
            {
                bindingPath = "triggerValue",
                highValue = 1f,
                lowValue = 0f,
                showMixedValue = true,
            };
            triggerSlider.label = $"TriggerValue {triggerSlider.value}";
            triggerSlider.RegisterCallback<ChangeEvent<float>>(OnTriggerSliderChange);

            root.Add(gripSlider);
            root.Add(triggerSlider);

            var rootButton = new Button
            {
                text = "Go to root",
            };
            rootButton.clicked += GoToRoot;
            root.Add(rootButton);
            return root;
        }

        void GoToRoot()
        {
            Selection.activeGameObject = ((HandController)target).ControllerRoot;
            SceneView.lastActiveSceneView.FrameSelected();
        }

        void OnGripSliderChange(ChangeEvent<float> changeEvent)
        {
            var element = (Slider)changeEvent.target;
            element.label = $"GripValue {changeEvent.newValue}";

            HandController handController = (HandController)target;
            handController.UpdateGrip(changeEvent.newValue);
        }

        void OnTriggerSliderChange(ChangeEvent<float> changeEvent)
        {
            var element = (Slider)changeEvent.target;
            element.label = $"GripValue {changeEvent.newValue}";

            HandController handController = (HandController)target;
            handController.UpdateTrigger(changeEvent.newValue);
        }

        private void HandleButton1Click()
        {
            // Handle button 1 click action
        }

        private void HandleButton2Click()
        {
            // Handle button 2 click action
        }
    }
}
