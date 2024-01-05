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
    public class HandControllerEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            root.Add(new PropertyField { label = "XrHand", bindingPath = "xrHand" });

            root.AddHeader("Configuration");

            root.Add(new PropertyField(serializedObject.FindProperty("controllerConfiguration")));

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
            element.label = $"Grip {changeEvent.newValue}";

            HandController handController = (HandController)target;
            handController.UpdateGrip(changeEvent.newValue);
        }

        void OnTriggerSliderChange(ChangeEvent<float> changeEvent)
        {
            var element = (Slider)changeEvent.target;
            element.label = $"Trigger {changeEvent.newValue}";

            HandController handController = (HandController)target;
            handController.UpdateTrigger(changeEvent.newValue);
        }
    }
}
