using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using XrCore.XrPhysics.Hands;

namespace XrCore.Interaction.Control
{
    public class HandController : MonoBehaviour
    {
        public InputActionReference MainButtonAction;
        public InputActionReference SecondaryButtonAction;

        public InputActionProperty gripAction;
        public InputActionProperty triggerActionProperty;

        private void Start()
        {
            gripAction.action.performed += UpdateGrip;
            triggerActionProperty.action.performed += UpdateTrigger;

            MainButtonAction.action.performed += OnMainButtonDown;
            MainButtonAction.action.canceled += OnMainButtonUp;

            SecondaryButtonAction.action.performed += OnSecondaryButtonDown;
            SecondaryButtonAction.action.canceled += OnSecondaryButtonUp;
        }

        public XrHand xrHand;
        public GameObject ControllerRoot => xrHand.gameObject ?? gameObject;
        private IXrHandControls controls => xrHand;

        public void UpdateGrip(InputAction.CallbackContext callbackContext) => UpdateGrip(callbackContext.ReadValue<float>());
        public void UpdateGrip(float newValue) => controls.UpdateGrip(newValue);
        public float ReadGrip() => controls.ReadGrip();
        public void UpdateTrigger(InputAction.CallbackContext callbackContext) => UpdateTrigger(callbackContext.ReadValue<float>());
        public void UpdateTrigger(float newValue) => controls.UpdateTrigger(newValue);
        public float ReadTrigger() => controls.ReadTrigger();

        public void OnMainButtonDown(InputAction.CallbackContext callbackContext) => OnMainButtonDown();
        public void OnMainButtonDown()
        {
            Debug.Log("Main input recieved");
            controls.OnMainButtonDown();
        }

        public void OnMainButtonUp(InputAction.CallbackContext callbackContext) => OnMainButtonUp();
        public void OnMainButtonUp() => controls.OnMainButtonUp();
        public void OnSecondaryButtonDown(InputAction.CallbackContext callbackContext) => OnSecondaryButtonDown();
        public void OnSecondaryButtonDown()
        {
            Debug.Log("Secondary input recieved");
            controls.OnSecondaryButtonDown();
        }

        public void OnSecondaryButtonUp(InputAction.CallbackContext callbackContext) => OnSecondaryButtonUp();
        public void OnSecondaryButtonUp() => controls.OnSecondaryButtonUp();
    }

    public interface IXrHandControls
    {
        public void UpdateGrip(float newValue);
        public float ReadGrip();
        public void UpdateTrigger(float newValue);
        public float ReadTrigger();
        public void OnMainButtonDown();
        public void OnMainButtonUp();
        public void OnSecondaryButtonDown();
        public void OnSecondaryButtonUp();
    }
}
