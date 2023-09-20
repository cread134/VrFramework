using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlInput : MonoBehaviour
{
    [Header("Control settings")]
    [SerializeField] private GameObject xrBrain;
    private IXrControls[] controlInterface;

    private void Start()
    {
        controlInterface = xrBrain.GetComponents<IXrControls>();
    }

    private bool overrideControls = false;

    public void SetControlOverrde(bool value)
    {
        overrideControls = value;
    }
    public void OnMoveDelta(InputAction.CallbackContext context)
    {
        if (!overrideControls)
        {
            foreach (IXrControls control in controlInterface)
            {
                control.MoveDelta(context.ReadValue<Vector2>());
            }
        }
     //   Debug.Log("move delta");
    }

    public void OnRightMoveDelta(InputAction.CallbackContext context)
    {
      //  Debug.Log("Rightdelta");
        foreach (IXrControls control in controlInterface)
        {
            control.RightDelta(context.ReadValue<Vector2>());
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            foreach (IXrControls control in controlInterface)
            {
                control.OnJumpKey();
            }
        }
    }
}
