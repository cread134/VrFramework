using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEditor.XR.LegacyInputHelpers;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction;

public class XrBrain : MonoBehaviour, IXrControls
{
    private XROrigin m_xrorigin;
    private PlayerBodyController m_playerBodyController;


    [Header("Rotation settings")]
    [SerializeField] private Transform turnHolder;
    [SerializeField] private float snapRotateIncrement = 45f;
    [Space]
    [SerializeField] private Transform headTransform;
    [SerializeField] private Rigidbody headRigidbody;
    [SerializeField] private CapsuleCollider characterCollider;
    [SerializeField] private float overHeadSpace = 0.1f;
     private float targetPlayerHeight = 1.84f; //we don't want the character collider to stretch beyond this height
    
    private float MAX_HEIGHTALLOWANCE = 0.1f;

    private void Start()
    {
        m_xrorigin = GetComponent<XROrigin>();
        m_playerBodyController = GetComponent<PlayerBodyController>();
        CalibrateCharacter();
    }

    public float PlayerHeight()
    {
        return targetPlayerHeight;
    }
    private void Update()
    {
        AllignColliderHeight();
    }


    public void CalibrateCharacter()
    {
        targetPlayerHeight = m_xrorigin.CameraYOffset;
        m_playerBodyController.OnCalibrationUpdate(targetPlayerHeight, 0f);
    }

    public void SnapTurn(float direction)
    {
        float turnAmount = snapRotateIncrement * direction;
        turnHolder.transform.rotation = turnHolder.transform.rotation * Quaternion.AngleAxis(turnAmount, Vector3.up);

        Debug.Log("snapTurn ");
    }

    private void AllignColliderHeight()
    {
        Vector3 heightDifference = headTransform.position - characterCollider.transform.position;

        if (heightDifference.y > 0 && heightDifference.y > targetPlayerHeight + MAX_HEIGHTALLOWANCE)
        {
            float newHeight = heightDifference.y + overHeadSpace;
            characterCollider.height = newHeight;
            characterCollider.center = new Vector3(0f, newHeight / 2f, 0f);
        }
    }
    #region input handling
    public void MoveDelta(Vector2 delta)
    {
        
    }

    public void RightDelta(Vector2 delta)
    {
        if (delta.x != 0f)
        {
            SnapTurn(delta.x);
        }
    }

    public void TriggerRight(float value)
    {
    }

    public void TriggerLeft(float value)
    {
   
    }

    public void GripLeft(float value)
    {
      
    }

    public void GripRight(float value)
    {

    }

    public void OnJumpKey()
    {
     
    }
    #endregion
}
