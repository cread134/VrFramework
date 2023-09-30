using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;
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
            root.Add(new PropertyField { label = "Controller", bindingPath = "Controller" });

            var gripSlider = new Slider
            {
                bindingPath = "gripValue",
                highValue = 1f,
                lowValue = 0f,
                showMixedValue = true,
            };
            gripSlider.label = $"GripValue {gripSlider.value}";
            gripSlider.RegisterCallback<ChangeEvent<float>>(OnGripSliderChange);
            root.Add(gripSlider);

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
