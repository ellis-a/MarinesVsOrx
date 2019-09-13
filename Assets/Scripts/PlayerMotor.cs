using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    [SerializeField]
    private Camera _cam;

    private Vector3 _velocity = Vector3.zero;
    private Vector3 _rotation = Vector3.zero;
    private float _cameraRotationX = 0f;
    private float _currentCameraRotationX = 0f;
    private Vector3 _jumpForce = Vector3.zero;

    [SerializeField]
    private float _cameraRotationLimit = 90f;

    private Rigidbody _rigidBody;

    // Use this for initialization
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 velocity)
    {
        _velocity = velocity;
    }

    public void Rotate(Vector3 rotation)
    {
        _rotation = rotation;
    }

    public void RotateCamera(float cameraRotationX)
    {
        _cameraRotationX = cameraRotationX;
    }

    public void DoJump(Vector3 jumpForce)
    {
        _jumpForce = jumpForce;
    }

    private bool IsGrounded()
    {
        float distanceToGround = GetComponent<Collider>().bounds.extents.y;
        return Physics.Raycast(transform.position, -Vector3.up, distanceToGround + 0.1f);
    }

    void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }

    private void PerformMovement()
    {

        if (_velocity != Vector3.zero)
        {
            _rigidBody.MovePosition(_rigidBody.position + _velocity * Time.fixedDeltaTime);
        }
        if (IsGrounded())
        {
            if (_jumpForce != Vector3.zero)
            {
                Vector3 jumpForce = _jumpForce + _velocity;
                _rigidBody.AddForce(jumpForce * Time.fixedDeltaTime, ForceMode.Impulse);
            }
        }
    }

    private void PerformRotation()
    {
        _rigidBody.MoveRotation(_rigidBody.rotation * Quaternion.Euler(_rotation));
        if (_cam != null)
        {
            _currentCameraRotationX -= _cameraRotationX;
            _currentCameraRotationX = Mathf.Clamp(_currentCameraRotationX, -_cameraRotationLimit, _cameraRotationLimit);

            _cam.transform.localEulerAngles = new Vector3(_currentCameraRotationX, 0f, 0f);
        }
    }
}
