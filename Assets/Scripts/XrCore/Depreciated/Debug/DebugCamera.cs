using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugCamera : MonoBehaviour
{
    [Header("Camera settings")]
    [SerializeField] public Transform playerBody;
    [SerializeField] private Vector2 mouseSensitity;
    [SerializeField] private Vector2 smoothing;
    [SerializeField] private Vector2 clampInDegrees = new Vector2(360f, 170f);

    private Vector2 _mouseDelta;
    private Vector2 _smoothMouse;
    private Vector2 _mouseAbsolute;
    [SerializeField] private Vector2 startAbsolute = new Vector2(-90, 90);
    private Vector2 targetDirection = Vector2.zero;
    private Vector2 targetCharacterDirection;

    private void Awake()
    {
        if(!Application.isEditor)
        {
            Destroy(gameObject);
        }
    }
    private DebugController debugController;

    // Start is called before the first frame update
    void Start()
    {
        debugController = GameManager.Instance.debugController;
        _mouseAbsolute = startAbsolute;
    }
    public void MouseDelta(Vector2 delta)
    {
        Debug.Log(delta);
        _mouseDelta = delta;
    }
    // Update is called once per frame

    void Update()
    {
        if(Input.GetKey(KeyCode.G))
        {
            UpdateDebugCam();
            SetCursorLock(true);
        } else
        {
            SetCursorLock(false);
        }
    }
    private bool _isCursorLocked = false;
    public void SetCursorLock(bool locked)
    {
        if (_isCursorLocked == locked)
        {
            return;
        }
        _isCursorLocked = locked;
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.Confined;
        Cursor.visible = !locked;
    }

    private void UpdateDebugCam()
    {
        var targetOrientation = Quaternion.Euler(targetDirection);
        var targetCharacterOrientation = Quaternion.Euler(targetCharacterDirection);
        _mouseDelta = Vector2.Scale(_mouseDelta, new Vector2(mouseSensitity.x * smoothing.x, mouseSensitity.y * smoothing.y));
        _smoothMouse.x = Mathf.Lerp(_smoothMouse.x, _mouseDelta.x, 1f / smoothing.x);
        _smoothMouse.y = Mathf.Lerp(_smoothMouse.y, _mouseDelta.y, 1f / smoothing.y);
        _mouseAbsolute += _smoothMouse;
        if (clampInDegrees.x < 360) _mouseAbsolute.x = Mathf.Clamp(_mouseAbsolute.x, -clampInDegrees.x * 0.5f, clampInDegrees.x * 0.5f);
        if (clampInDegrees.y < 360) _mouseAbsolute.y = Mathf.Clamp(_mouseAbsolute.y, -clampInDegrees.y * 0.5f, clampInDegrees.y * 0.5f);
        transform.localRotation = (Quaternion.AngleAxis(-_mouseAbsolute.y, targetOrientation * Vector3.right) * targetOrientation);
        var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, Vector3.up);
        playerBody.transform.rotation = yRotation * targetCharacterOrientation;
    }
}
