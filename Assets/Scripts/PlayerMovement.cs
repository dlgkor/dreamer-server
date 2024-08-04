using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
<<<<<<< HEAD
using UnityEngine.SocialPlatforms;
=======
>>>>>>> 8c8aeda9a18718168c684636512b582fe59e3c7a

public class PlayerMovement : MonoBehaviour
{
    public GameObject Orientation;
    public GameObject LookDirection;
    public LocalInput LocalInput;

    //힘에 대한 정보
 
    public float forwardForce;
    public float horizontalForce;
    public float horizontalMoveCoefficient = 10f;
    public float forwardMoveCoefficient = 10f;
<<<<<<< HEAD
    public float speedLimit = 10f;

=======
>>>>>>> 8c8aeda9a18718168c684636512b582fe59e3c7a
    public float xRotationSpeed;
    public float yRotationSpeed;
    public float JumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    private bool readyToJump = true; 
    bool grounded;
    public LayerMask Ground;
    private Rigidbody rb;
    private float xRotation;
    private float yRotation;

    public float mouseSensitivity = 100f;


    void Start()
    {
        forwardForce = 0;
        horizontalForce = 0;
        xRotationSpeed = 0;
        yRotationSpeed = 0;
        JumpForce = 10f;
        jumpCooldown = 0.1f;
        airMultiplier = 0.4f;
        rb = GetComponent<Rigidbody>();
        xRotation = 0;
        yRotation = 0;
    }

    void Update()
    {
<<<<<<< HEAD
        if (LocalInput.isJumpPressed() && readyToJump && grounded)
        {
            Jump();
        }

        grounded = Physics.Raycast(transform.position, Vector3.down, Ground);
    }


    void FixedUpdate()
    {
        if(grounded)
        {
            Vector3 forwardDir = Orientation.transform.forward;
            rb.AddForce(forwardDir.normalized * forwardForce * forwardMoveCoefficient);
            Vector3 horizontalDir = Orientation.transform.right;
            rb.AddForce(horizontalDir.normalized * horizontalForce * horizontalMoveCoefficient);
        }

        else
        {
            Vector3 forwardDir = Orientation.transform.forward;
            rb.AddForce(forwardDir.normalized * forwardForce * airMultiplier * forwardMoveCoefficient);
            Vector3 horizontalDir = Orientation.transform.right;
            rb.AddForce(horizontalDir.normalized * horizontalForce * airMultiplier *horizontalMoveCoefficient);
        }

        SpeedControl();

        xRotation -= xRotationSpeed * Time.fixedDeltaTime * mouseSensitivity;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); //상하 시선이동 범위 제한
        yRotation += yRotationSpeed * Time.fixedDeltaTime * mouseSensitivity;
=======
        Vector3 forwardDir = Orientation.transform.forward;
        rb.AddForce(forwardDir.normalized*forwardForce*forwardMoveCoefficient);
        Vector3 horizontalDir = Orientation.transform.right;
        rigidBody.AddForce(horizontalDir.normalized*horizontalForce*horizontalMoveCoefficient);
        rb.AddForce(rb.velocity * rho * -1.0f);
        xRotation += xRotationSpeed;
        yRotation += yRotationSpeed;
>>>>>>> 8c8aeda9a18718168c684636512b582fe59e3c7a
        Orientation.transform.rotation = Quaternion.Euler(0,yRotation,0);
        LookDirection.transform.rotation = Quaternion.Euler(xRotation,yRotation,0);
    }

    private void SpeedControl()
    {
<<<<<<< HEAD
        //속도제한
        Vector3 currentVelocity = rb.velocity;
        if (currentVelocity.magnitude > speedLimit)
        {
            rb.velocity = currentVelocity.normalized * speedLimit;
        }
    }

    private void Jump()
    {
        if(readyToJump)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            Vector3 JumpDir = Orientation.transform.up;
            rb.AddForce(JumpDir * JumpForce, ForceMode.Impulse);
            readyToJump = false; // 점프 후 재설정 필요
            Invoke(nameof(ResetJump), jumpCooldown); // 짧은 시간 후 점프 재설정
        }
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
=======
    // 현재 Rigidbody의 속도 벡터
    Vector3 currentVelocity = rigidBody.velocity;

    // XZ 평면의 속도 벡터 (평행이동 속도)
    Vector3 flatVel = new Vector3(currentVelocity.x, 0f, currentVelocity.z);

    // 앞뒤 이동 속도 제한
    float forwardSpeedLimit = 10f; // 앞뒤 이동 속도의 상한선
    float limitedForwardVel = Mathf.Clamp(currentVelocity.y, -forwardSpeedLimit, forwardSpeedLimit);

    // 평행이동 속도 제한
    float moveSpeedLimit = 10f; // 평행이동 속도의 상한선
    if (flatVel.magnitude > moveSpeedLimit)
    {
        flatVel = flatVel.normalized * moveSpeedLimit;
    }

    // 제한된 속도를 Rigidbody에 적용
    rigidBody.velocity = new Vector3(flatVel.x, limitedForwardVel, flatVel.z);
    
    }
    
>>>>>>> 8c8aeda9a18718168c684636512b582fe59e3c7a
}
