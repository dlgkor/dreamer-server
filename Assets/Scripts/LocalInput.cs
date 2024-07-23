using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalInput : MonoBehaviour
{

    private float mouseHorizontal;
    private float mouseVertical;

    public PlayerMovement playerMovement;
    public float keyVertical;
    public float keyHorizontal;

    private bool cursorLocked = true;


    void Start()
    {
        mouseHorizontal = 0;
        mouseVertical = 0;
        playerMovement = GetComponent<PlayerMovement>();
        keyVertical = 0;
        keyHorizontal = 0;
        ToggleCursorLock(true);
    }


    void Update()
    {
        mouseHorizontal = Input.GetAxisRaw("Mouse X");
        mouseVertical = Input.GetAxisRaw("Mouse Y");
        playerMovement.xRotationSpeed = mouseVertical;
        playerMovement.yRotationSpeed = mouseHorizontal;
        keyVertical = Input.GetAxisRaw("Vertical");
        keyHorizontal = Input.GetAxisRaw("Horizontal");
        playerMovement.forwardForce = keyVertical;
        playerMovement.horizontalForce = keyHorizontal;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCursorLock(!cursorLocked);
        }
    }

    void ToggleCursorLock(bool isLocked)
    {
        cursorLocked = isLocked;
        if (cursorLocked)
        {
            Debug.Log("Locked mouse");
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
