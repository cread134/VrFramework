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
        public ControllerConfiguration controllerConfiguration;

        private void Start()
        {
            controllerConfiguration.gripAction.action.performed += UpdateGrip;
            controllerConfiguration.triggerActionProperty.action.performed += UpdateTrigger;

            controllerConfiguration.mainButtonAction.action.performed += OnMainButtonDown;
            controllerConfiguration.mainButtonAction.action.canceled += OnMainButtonUp;

            controllerConfiguration.secondaryButtonAction.action.performed += OnSecondaryButtonDown;
            controllerConfiguration.secondaryButtonAction.action.canceled += OnSecondaryButtonUp;
        }

        public XrHand xrHand;
        public GameObject ControllerRoot => xrHand.gameObject ?? gameObject;
        private IXrHandControls Controls => xrHand;

        #region grip
        public void UpdateGrip(InputAction.CallbackContext callbackContext) => UpdateGrip(callbackContext.ReadValue<float>());
        bool gripCrossedThresshold = false;
        public void UpdateGrip(float newValue)
        {
            if (newValue < controllerConfiguration.gripThreshold)
            {
                if (gripCrossedThresshold != false)
                {
                    Controls.OnGripUp();
                }
                gripCrossedThresshold = false;
            }
            else
            {
                if (gripCrossedThresshold != true)
                {
                    Controls.OnGripDown();
                }
                gripCrossedThresshold = true;
            }
            Controls.UpdateGrip(newValue);
        }

        public float ReadGrip() => Controls.ReadGrip();
        #endregion

        #region trigger
        public void UpdateTrigger(InputAction.CallbackContext callbackContext) => UpdateTrigger(callbackContext.ReadValue<float>());

        bool triggerCrossedThresshold = false;
        public void UpdateTrigger(float newValue)
        {
            if (newValue < controllerConfiguration.triggerThreshold)
            {
                if (triggerCrossedThresshold != false)
                {
                    Controls.OnTriggerUp();
                }
                triggerCrossedThresshold = false;
            }
            else
            {
                if (triggerCrossedThresshold != true)
                {
                    Controls.OnTriggerDown();
                }
                triggerCrossedThresshold = true;
            }
            Controls.UpdateTrigger(newValue);
        }
        public float ReadTrigger() => Controls.ReadTrigger();
        #endregion


        public void OnMainButtonDown(InputAction.CallbackContext callbackContext) => OnMainButtonDown();
        public void OnMainButtonDown()
        {
            Controls.OnMainButtonDown();
        }

        public void OnMainButtonUp(InputAction.CallbackContext callbackContext) => OnMainButtonUp();
        public void OnMainButtonUp()
        {
            Controls.OnMainButtonUp();
        }

        public void OnSecondaryButtonDown(InputAction.CallbackContext callbackContext) => OnSecondaryButtonDown();
        public void OnSecondaryButtonDown()
        {
            Controls.OnSecondaryButtonDown();
        }

        public void OnSecondaryButtonUp(InputAction.CallbackContext callbackContext) => OnSecondaryButtonUp();
        public void OnSecondaryButtonUp() => Controls.OnSecondaryButtonUp();
    }

    public interface IXrHandControls
    {
        public void UpdateGrip(float newValue);
        public void OnGripDown();
        public void OnGripUp();
        public float ReadGrip();
        public void UpdateTrigger(float newValue);
        public void OnTriggerDown();
        public void OnTriggerUp();

        public float ReadTrigger();
        public void OnMainButtonDown();
        public void OnMainButtonUp();
        public void OnSecondaryButtonDown();
        public void OnSecondaryButtonUp();
    }
}
