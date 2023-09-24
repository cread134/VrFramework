using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEngine.XR;

public class DebugController : MonoBehaviour
{
    private ControlInput m_ControlInput;

    [SerializeField]private bool m_DebugEnabled = true;
    [Space]
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float gripChangeDelta = 10f;

    private Vector2 moveDelta;
    private Vector2 mouseDelta;

    private float verticalDelta;

    public InputActionReference moveAction;
    public InputActionReference mouseDeltaAction;

    private XrHand moveTarget;

    private bool targeting = false;

    private void OnEnable()
    {
        if(!Application.isEditor)
        {
            m_DebugEnabled = false;
        }

        moveAction.action.performed += MoveDelta;
        mouseDeltaAction.action.performed += MouseDelta;
    }

    private void Start()
    {
        debugCamera = GameObject.FindAnyObjectByType<DebugCamera>();
        m_ControlInput = FindAnyObjectByType<ControlInput>();
    }

    private void Update()
    {
        if (m_DebugEnabled)
        {
            DebugLoop();
        }
    }

    private void DebugLoop()
    {
        if(moveTarget != null && targeting)
        {
            MoveTarget();   
            RotateTarget();
        }
    }
    
    void MoveTarget()
    {
            Transform trackTarget = moveTarget.TrackingTarget();
            Vector3 move = new Vector3(moveDelta.x, verticalDelta, moveDelta.y);
            trackTarget.position += move * moveSpeed * Time.deltaTime;
    }

    void RotateTarget()
    {
        Transform trackTarget = moveTarget.TrackingTarget();
        trackTarget.Rotate(Vector3.up * Time.deltaTime * rotationSpeed * mouseDelta.x);
        trackTarget.Rotate(Vector3.right * Time.deltaTime * rotationSpeed * mouseDelta.y);

    }

    public void SelectController(XrHand targetHand)
    {
        moveTarget = targetHand;
        targeting = true;


    }
    public void DeselectController(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Deselect();
        }
    }

    private void Deselect()
    {
        targeting = false;
    }

    public void MoveDelta(InputAction.CallbackContext context)
    {
        moveDelta = context.ReadValue<Vector2>();
    }

    public void MouseDelta(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
        debugCamera?.MouseDelta(mouseDelta);   
    }
    private DebugCamera debugCamera;



    public void VerticalDelta(InputAction.CallbackContext context)
    {
        verticalDelta = context.ReadValue<float>();
    }
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        if(targeting )
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(moveTarget.TrackingTarget().position, 0.02f);

            
        }

    }

    public void UpdateGripValue(InputAction.CallbackContext context)
    {
        if(targeting )
        {
            moveTarget.UpdateGripValue(Mathf.Clamp01(moveTarget.ReadGripValue() + (context.ReadValue<float>() * gripChangeDelta)));

            
        }
    }
}
