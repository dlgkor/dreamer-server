using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    private Transform Orientation;
    private Transform LookDirection;

    //힘에 대한 정보
 
    public float forwardForce;
    public float horizontalForce;
    public float horizontalMoveCoefficient = 10f;
    public float forwardMoveCoefficient = 10f;
    public float speedLimit = 10f;

    public float xRotationSpeed;
    public float yRotationSpeed;
    public float JumpForce;

    private Rigidbody rb;
    private float xRotation;
    private float yRotation;

    public float mouseSensitivity = 100f;


    void Start()
    {
        LookDirection = transform.Find("LookDirection");
        Orientation = transform.Find("Orientation");

        forwardForce = 0;
        horizontalForce = 0;
        xRotationSpeed = 0;
        yRotationSpeed = 0;
        JumpForce = 0;
        rb = GetComponent<Rigidbody>();
        xRotation = 0;
        yRotation = 0;
    }


    void FixedUpdate()
    {
        Vector3 forwardDir = Orientation.forward;
        rb.AddForce(forwardDir.normalized * forwardForce * forwardMoveCoefficient);
        Vector3 horizontalDir = Orientation.right;
        rb.AddForce(horizontalDir.normalized * horizontalForce * horizontalMoveCoefficient);
        SpeedControl();

        xRotation -= xRotationSpeed * Time.fixedDeltaTime * mouseSensitivity;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); //상하 시선이동 범위 제한
        yRotation += yRotationSpeed * Time.fixedDeltaTime * mouseSensitivity;
        Orientation.rotation = Quaternion.Euler(0,yRotation,0);
        LookDirection.rotation = Quaternion.Euler(xRotation,yRotation,0);
        
        GetComponent<PlayerPoint>()?.AddPoints((Mathf.Abs(forwardForce * 100f)
            + Mathf.Abs(horizontalForce * 100f)
            + Mathf.Abs(xRotationSpeed * 5f) + Mathf.Abs(yRotationSpeed * 5f)) * (-1) * Time.fixedDeltaTime * 0.0001f);
        
        
    }

    private void SpeedControl()
    {
        //속도제한
        Vector3 currentVelocity = rb.velocity;
        if (currentVelocity.magnitude > speedLimit)
        {
            rb.velocity = currentVelocity.normalized * speedLimit;
        }
    }
}
