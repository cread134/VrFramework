using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XrLocomotion : MonoBehaviour, IXrControls
{
    [SerializeField] private XrBrain xrController;
  
    [Header("Movementcomponents")]
    [SerializeField] private float walkSpeed = 8f;
    [SerializeField] private float maxAcceleration = 1f;
    [SerializeField] private Rigidbody characterBody;
    [SerializeField] private Transform moveTransform;
    [Space]
    [SerializeField] private LayerMask groundedMask;
    [SerializeField] private float groundedCheckRadius = 0.1f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpCooldown= 0.1f;
    [SerializeField] private float jumpForce = 1f;

    [Header("AlligningThe body")]
    [SerializeField] private LayerMask headCollisionMask;
    [SerializeField] private float headRadius = 0.3f;
    [SerializeField] private float maxDistanceError = 1f;
    [SerializeField] private Transform targetHead;
    [SerializeField] private Rigidbody headBody;
    [SerializeField] private float positionGain = 1f; // Proportional gain for position control
    [SerializeField] private float rotationGain = 1f; // Proportional gain for rotation control
    [SerializeField] private float bodyPositionGain = 100f; // Proportional gain for position control
    [SerializeField] private float bodyRotationGain = 100f; // Proportional gain for rotation control
    [Space]
    [SerializeField][Tooltip("the max distance to allow the head to be separated from body")] private float maxHeadBodyOffset = 0.3f;
    [SerializeField] private float headFollowheightAllowance = 0.1f;


    //private fields
    private Vector2 targetDelta;
    private Vector3 positionError;
    private Vector3 bodyPositionError;
    private Vector3 previousPositionError;
    private Vector3 previousBodyPositionError;
    private Vector3 integralError;
    private Vector3 bodyIntegralError;
    private Vector3 appliedVelocity;

    private bool grounded;
    private bool headCollision;

    private float lastJump;
    private float distanceError;

    #region input handling
    public void GripLeft(float value)
    {
        throw new System.NotImplementedException();
    }

    public void GripRight(float value)
    {
    }

    public void MoveDelta(Vector2 delta)
    {
        targetDelta = delta;
    }
    public void RightDelta(Vector2 delta)
    {
       
    }

    public void OnJumpKey()
    {
        Jump();
    }

    public void TriggerLeft(float value)
    {
    }

    public void TriggerRight(float value)
    {
    }

    #endregion
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        UpdateMovement();
        MoveCharacter();
    }

    private void FixedUpdate()
    {
        MatchPhysicsHead();
        MoveCharacterToMatchHead();
    }

    void UpdateMovement()
    {
        if (characterBody == null) return;

        //checkgrounded
        grounded = Physics.CheckSphere(characterBody.transform.position, groundedCheckRadius, groundedMask);

        if (!grounded)
        {
            appliedVelocity.y += (gravity * Time.deltaTime);
        }
        else
        {
            if(appliedVelocity.y < 0)
            {
                appliedVelocity.y = 0;
            }
        }
        moveTransform.Translate(appliedVelocity * Time.deltaTime);
    }

    void MoveCharacter()
    {
        if (grounded)
        {
            Vector3 move = headBody.transform.right * targetDelta.x + headBody.transform.forward * targetDelta.y;
            move.y = 0f;
            Vector3 forceAdd =move * walkSpeed;
            Vector3 acceleration = forceAdd /  Time.fixedDeltaTime;
            acceleration = Vector3.ClampMagnitude(acceleration, maxAcceleration);

       
            moveTransform.Translate(acceleration * Time.deltaTime);
        }
    }

    void MatchPhysicsHead()
    {
        Vector3 targetHeadPos = targetHead.position;

        headCollision = Physics.CheckSphere(headBody.position, headRadius, headCollisionMask);
        distanceError = Vector3.Distance(headBody.position, targetHeadPos);

        Vector3 yAdjusted = new Vector3(characterBody.position.x, targetHead.position.y, characterBody.position.z);
        Debug.DrawLine(targetHeadPos, yAdjusted, Color.red, Time.deltaTime);
        float headBodyDistance = Vector3.Distance(yAdjusted, targetHeadPos);

        if (headBodyDistance > maxHeadBodyOffset)
        {
            Vector3 dif = (targetHead.position - yAdjusted).normalized * maxHeadBodyOffset;
            targetHeadPos = yAdjusted + dif;
        }

        if (headCollision && distanceError < maxDistanceError)
        {
            if(headBody.isKinematic) { headBody.isKinematic = false; }
            // Calculate position error
            positionError = targetHeadPos - headBody.position;

            // Calculate integral error
            integralError += positionError * Time.fixedDeltaTime;

            // Calculate derivative error
            Vector3 derivativeError = (positionError - previousPositionError) / Time.fixedDeltaTime;

            // Calculate PID control
            Vector3 force = positionGain * positionError + integralError + rotationGain * derivativeError;

            // Apply force to the rigidbody
            headBody.AddForce(force);

            // Save current position error for the next frame
            previousPositionError = positionError;
        }
        else
        {
            if(!headBody.isKinematic) { headBody.isKinematic = true; }
            headBody.MovePosition(targetHeadPos);
        }

        // Match rotation
        headBody.MoveRotation(targetHead.rotation);
     
    }

    void MoveCharacterToMatchHead()
    {
        //match positional
        Vector3 targetPosition = new Vector3(headBody.position.x, 0f, headBody.position.z);
        Vector3 currentHorizontal = new Vector3(characterBody.position.x, 0f, characterBody.position.z);

        float verticalDistance = headBody.position.y - characterBody.position.y;

        if(verticalDistance > xrController.PlayerHeight() + headFollowheightAllowance || !grounded)
        {
            float verticalOffset = verticalDistance - xrController.PlayerHeight();
            targetPosition.y = verticalOffset;

            characterBody.useGravity = false;
        }
        else
        {
            characterBody.useGravity = true;
        }

        if (Vector3.Distance(targetPosition, characterBody.position) > 0.1f)
        {

            bodyPositionError = targetPosition - currentHorizontal;

            // Calculate integral error
            bodyIntegralError += bodyPositionError * Time.fixedDeltaTime;

            // Calculate derivative error
            Vector3 derivativeError = (bodyPositionError - previousBodyPositionError) / Time.fixedDeltaTime;

            // Calculate PID control
            Vector3 force = bodyPositionGain * bodyPositionError + bodyIntegralError + bodyRotationGain * derivativeError;
            // Save current position error for the next frame
            previousBodyPositionError = bodyPositionError;

            characterBody.AddForce(force);
        }
        else
        {
            characterBody.MovePosition(targetPosition);
        }



        //allignbody rotation (to be implemented properly)
        Quaternion targetRotation = Quaternion.LookRotation(headBody.transform.forward, Vector3.up);
        characterBody.MoveRotation(targetRotation);
    }

    void Jump()
    {
        if(grounded && Time.time > lastJump)
        {
            Debug.Log("Character Jump");
            lastJump = Time.time + jumpCooldown;
            grounded = false;
            //characterBody.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);
            appliedVelocity.y = jumpForce;
        }
        else
        {
            Debug.Log("Unable to jump");
        }
    }

    public void OnDrawGizmos()
    {
        if (grounded)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawWireSphere(characterBody.transform.position, groundedCheckRadius);


        if (Application.isPlaying)
        {

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(targetHead.transform.position, 0.05f);
            Gizmos.DrawLine(targetHead.transform.position, targetHead.transform.position + (targetHead.transform.forward * 0.1f));
            Gizmos.color = Color.blue;

            if (headCollision && distanceError < maxDistanceError)
            {
                Gizmos.color = Color.red;
            }
                Gizmos.DrawSphere(headBody.position, 0.03f);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(characterBody.position + new Vector3(0f, xrController.PlayerHeight() / 2f, 0f), new Vector3(0.3f, xrController.PlayerHeight(), 0.3f));
        }
    }
}
