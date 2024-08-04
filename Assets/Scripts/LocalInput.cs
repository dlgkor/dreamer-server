using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalInput : MonoBehaviour
{

    private float mouseHorizontal;
    private float mouseVertical;

    public PlayerMovement playerMovement;
    public float keyVertical;
<<<<<<< HEAD
    public float keyHorizontal;
=======
    public float KeyHorizontal;
>>>>>>> 8c8aeda9a18718168c684636512b582fe59e3c7a
    private bool cursorLocked = true;
    public bool isJumpPressed()
    {
        return Input.GetKeyDown(KeyCode.Space); // TAB 바를 점프 입력으로 사용합니다.
    }


    void Start()
    {
        mouseHorizontal = 0;
        mouseVertical = 0;
        playerMovement = GetComponent<PlayerMovement>();
        keyVertical = 0;
<<<<<<< HEAD
        keyHorizontal = 0;
=======
        KeyHorizontal = 0;
>>>>>>> 8c8aeda9a18718168c684636512b582fe59e3c7a
        ToggleCursorLock(true);
    }


    void Update()
    {
        mouseHorizontal = Input.GetAxisRaw("Mouse X");
        mouseVertical = Input.GetAxisRaw("Mouse Y");
        playerMovement.xRotationSpeed = mouseVertical;
        playerMovement.yRotationSpeed = mouseHorizontal;
        keyVertical = Input.GetAxisRaw("Vertical");
        playerMovement.forwardForce = keyVertical;
<<<<<<< HEAD
        keyHorizontal = Input.GetAxisRaw("Horizontal");
        playerMovement.horizontalForce = keyHorizontal;
=======
        KeyHorizontal = Input.GetAxisRaw("Horizontal");
        playerMovement.horizontalForce = KeyHorizontal;
>>>>>>> 8c8aeda9a18718168c684636512b582fe59e3c7a

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //ESC Ű�� Ŀ�� ��� ���� ����
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
