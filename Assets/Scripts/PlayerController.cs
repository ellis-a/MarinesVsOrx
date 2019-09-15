using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private float _lookSensitivity = 3f;
    [SerializeField]
    private float _jumpForce = 200f;


    private PlayerMotor _motor;

    void Start()
    {
        _motor = GetComponent<PlayerMotor>();
    }

    void Update()
    {
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        float xMov = Input.GetAxisRaw("Horizontal");
        float zMov = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * xMov;
        Vector3 moveVertical = transform.forward * zMov;

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * _speed;

        _motor.Move(velocity);

        //player y rotation
        float yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 rotation = new Vector3(0f, yRotation, 0f) * _lookSensitivity;

        _motor.Rotate(rotation);

        //camera x rotation
        float xRotation = Input.GetAxisRaw("Mouse Y");
        float cameraRotationX = xRotation * _lookSensitivity;

        _motor.RotateCamera(cameraRotationX);

        //do jump
        Vector3 jumpForce = Vector3.zero;
        if (Input.GetButton("Jump"))
        {
            jumpForce = Vector3.up * _jumpForce;
        }

        _motor.DoJump(jumpForce);
    }
}
