using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCamera : MonoBehaviour
{
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
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.LeftAlt))
        {
            UpdateDebugCam();
        }
    }

    private void UpdateDebugCam()
    {

    }
}
