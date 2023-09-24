using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XrHand))]
public class HandController : MonoBehaviour
{
    public ActionBasedController Controller;

    public InputActionReference MainButtonAction;
    public InputActionReference SecondaryButtonAction;

    private void Awake()
    {
        Controller.selectAction.action.performed += UpdateGrip;
        Controller.activateAction.action.performed += UpdateTrigger;

        MainButtonAction.action.performed += OnMainButtonDown;
        MainButtonAction.action.canceled += OnMainButtonUp;

        SecondaryButtonAction.action.performed += OnSecondaryButtonDown;
        SecondaryButtonAction.action.canceled += OnSecondaryButtonUp;
    }

    private IXrHandControls controls;
    private void ValidateControls()
    {
        if (controls == null)
        {
            controls = GetComponent<IXrHandControls>();
        }
    }

    public float gripValue = 0f;
    public float triggerValue = 0f;

    public void UpdateGrip(InputAction.CallbackContext callbackContext) => UpdateGrip(callbackContext.ReadValue<float>());
    public void UpdateGrip(float newValue)
    {
        ValidateControls();
        controls.UpdateGrip(newValue);
    }

    public void UpdateTrigger(InputAction.CallbackContext callbackContext) => UpdateTrigger(callbackContext.ReadValue<float>());
    public void UpdateTrigger(float newValue)
    {
        ValidateControls();
        controls.UpdateTrigger(newValue);
    }

    public void OnMainButtonDown(InputAction.CallbackContext callbackContext) => OnMainButtonDown();
    public void OnMainButtonDown()
    {
        ValidateControls();
        controls.OnMainButtonDown();
    }
    public void OnMainButtonUp(InputAction.CallbackContext callbackContext) => OnMainButtonUp();
    public void OnMainButtonUp()
    {
        ValidateControls();
        controls.OnMainButtonUp();
    }
    public void OnSecondaryButtonDown(InputAction.CallbackContext callbackContext) => OnSecondaryButtonDown();
    public void OnSecondaryButtonDown()
    {
        ValidateControls();
        controls.OnSecondaryButtonDown();
    }
    public void OnSecondaryButtonUp(InputAction.CallbackContext callbackContext) => OnSecondaryButtonUp();
    public void OnSecondaryButtonUp()
    {
        ValidateControls();
        controls.OnSecondaryButtonUp();
    }
}

public interface IXrHandControls
{
    public void UpdateGrip(float newValue);
    public void UpdateTrigger(float newValue);
    public void OnMainButtonDown();
    public void OnMainButtonUp();
    public void OnSecondaryButtonDown();
    public void OnSecondaryButtonUp();
}
