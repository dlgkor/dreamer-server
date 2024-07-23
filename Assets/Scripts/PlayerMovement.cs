using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject Orientation;
    public GameObject LookDirection;

    //힘에 대한 정보
 
    public float forwardForce;
    public float horizontalForce;
    public float xRotationSpeed;
    public float yRotationSpeed;
    public float JumpForce;

    private Rigidbody rb;
    private float xRotation;
    private float yRotation;

    public float rho = 0;


    void Start()
    {
        LookDirection = transform.GetChild(0).gameObject;
        Orientation = transform.GetChild(1).gameObject;

        forwardForce = 0;
        horizontalForce = 0;
        xRotationSpeed = 0;
        yRotationSpeed = 0;
        JumpForce = 0;
        rb = GetComponent<Rigidbody>();
        xRotation = 0;
        yRotation = 0;
    }


    void Update()
    {
        Vector3 forwardDirection = Orientation.transform.forward;
        rb.AddForce(forwardDirection.normalized*forwardForce*10);
        Vector3 horizontalDirection = Orientation.transform.right;
        rb.AddForce(horizontalDirection.normalized * horizontalForce * 10);

        rb.AddForce(rb.velocity * rho * -1.0f);


        xRotation -= xRotationSpeed;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        yRotation += yRotationSpeed;
        Orientation.transform.rotation = Quaternion.Euler(0,yRotation,0);
        LookDirection.transform.rotation = Quaternion.Euler(xRotation,yRotation,0);

    }
}
