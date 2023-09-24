using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

[CustomEditor(typeof(HandController))]
public class XrControllerEditor : Editor
{
    public override VisualElement CreateInspectorGUI()
    {
        VisualElement root = new VisualElement();

        root.Add(new PropertyField { label = "Controller", bindingPath = "Controller" });

        var gripSlider = new Slider { 
            bindingPath = "gripValue",
            highValue = 1f, 
            lowValue = 0f,
            showMixedValue = true,
        };
        gripSlider.label = $"GripValue {gripSlider.value}"; 
        gripSlider.RegisterCallback<ChangeEvent<float>>(OnGripSliderChange);
        root.Add(gripSlider);
        return root;
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
