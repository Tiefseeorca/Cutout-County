using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    public static PlayerController Instance;
    public float speed;
    public float jumpForce;
    public float groundCheckDistance;
    [SerializeField] private float _distanceToGround;
    [SerializeField] private float _springStrength;
    [SerializeField] private float _springSoftness;

    private bool _inInteraction;
    [SerializeField] private bool _grounded;
    private float _mass;
    
    private Rigidbody _rb;
    private InputAction _walkAction;
    private InputAction _jumpAction;
    private Transform _cameraTransform;

    void Start() {
        Instance = this;
        _rb = GetComponent<Rigidbody>();
        _mass = _rb.mass;
        _walkAction = InputSystem.actions.FindAction("Move");
        _jumpAction = InputSystem.actions.FindAction("Jump");
        _cameraTransform = transform.GetChild(0);
    }

    void FixedUpdate(){
        Vector2 input = _walkAction.ReadValue<Vector2>();
        Vector3 moveDirection = _cameraTransform.forward * input.y + _cameraTransform.right * input.x;
        
        _rb.linearVelocity = new Vector3(moveDirection.x * speed, _rb.linearVelocity.y, moveDirection.z * speed );

        RaycastHit hit;
        _grounded = Physics.Raycast(transform.position, Vector3.down, out hit, _distanceToGround);
        if (_grounded) {
            _rb.mass = _mass;
            float relativeVeloctiy = Vector3.Dot(Vector3.down, _rb.linearVelocity);
            float x = hit.distance - _distanceToGround;
            float springForce = (x * _springStrength) - (relativeVeloctiy * _springSoftness);
            _rb.AddForce(Vector3.down * springForce);
            //_rb.AddForce(Vector3.up * (_springStrength * 1f / Mathf.Max(hit.distance * _springSoftness, 0.01f)));
        } else if (_rb.linearVelocity.y < 0) {
            _rb.mass = _mass * 2;
        }
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