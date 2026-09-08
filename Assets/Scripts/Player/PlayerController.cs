using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour{
    public float speed = 3f;
    public float jumpForce = 5f;
    public float groundCheckDistance = 1f;
    
    private Rigidbody _rb;
    private InputAction _walkAction;
    private InputAction _jumpAction;
    private Transform _cameraTransform;

    void Start(){
        _rb = GetComponent<Rigidbody>();
        _walkAction = InputSystem.actions.FindAction("Move");
        _jumpAction = InputSystem.actions.FindAction("Jump");
        _cameraTransform = transform.GetChild(0);
    }

    void FixedUpdate(){
        Vector2 input = _walkAction.ReadValue<Vector2>();
        Vector3 moveDirection = _cameraTransform.forward * input.y + _cameraTransform.right * input.x;
        
        _rb.linearVelocity = new Vector3(moveDirection.x * speed, _rb.linearVelocity.y, moveDirection.z * speed );
    }

    private void Update() {
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);
        bool jumpPressed = _jumpAction.WasPressedThisFrame();
        if (jumpPressed && isGrounded){
            _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z);
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}