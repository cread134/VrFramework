using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

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

    public XrHand xrHand;
    private IXrHandControls controls => xrHand;

    public float gripValue = 0f;
    public float triggerValue = 0f;

    public void UpdateGrip(InputAction.CallbackContext callbackContext) => UpdateGrip(callbackContext.ReadValue<float>());
    public void UpdateGrip(float newValue) => controls.UpdateGrip(newValue);

    public void UpdateTrigger(InputAction.CallbackContext callbackContext) => UpdateTrigger(callbackContext.ReadValue<float>());
    public void UpdateTrigger(float newValue) => controls.UpdateTrigger(newValue);

    public void OnMainButtonDown(InputAction.CallbackContext callbackContext) => OnMainButtonDown();
    public void OnMainButtonDown() => controls.OnMainButtonDown();
    public void OnMainButtonUp(InputAction.CallbackContext callbackContext) => OnMainButtonUp();
    public void OnMainButtonUp() => controls.OnMainButtonUp();
    public void OnSecondaryButtonDown(InputAction.CallbackContext callbackContext) => OnSecondaryButtonDown();
    public void OnSecondaryButtonDown() => controls.OnSecondaryButtonDown();
    public void OnSecondaryButtonUp(InputAction.CallbackContext callbackContext) => OnSecondaryButtonUp();
    public void OnSecondaryButtonUp() => controls.OnSecondaryButtonUp();
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
