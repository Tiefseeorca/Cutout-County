using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour {
    public static PlayerController Instance;
    public float speed;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _deceleration;
    public float jumpForce;
    [SerializeField] private float _distanceToGround;
    [SerializeField] private float _springStrength;
    [SerializeField] private float _springDamper;
    [SerializeField] private float _maxGrav;

    public bool InInteraction;
    [SerializeField] private bool _grounded;
    [SerializeField] private bool _inJump;
    private float _standardGravity;
    
    private Rigidbody _rb;
    private InputAction _walkAction;
    private InputAction _jumpAction;
    private Transform _cameraTransform;
    private POVCameraHelper _camera;
    
    void Awake() {
        Cursor.lockState = CursorLockMode.Locked;
        Instance = this;
        _rb = GetComponent<Rigidbody>();
        _walkAction = InputSystem.actions.FindAction("Move");
        _jumpAction = InputSystem.actions.FindAction("Jump");
        _cameraTransform = transform.GetChild(0).GetChild(0);
        _camera = _cameraTransform.GetComponent<POVCameraHelper>();
        _standardGravity = Physics.gravity.y;
    }

    void FixedUpdate() {
        /*if (!InInteraction) {
            Vector2 input = _walkAction.ReadValue<Vector2>();
            Vector3 moveDirection = _cameraTransform.forward * input.y + _cameraTransform.right * input.x;
            _rb.linearVelocity = new Vector3(moveDirection.x * speed, _rb.linearVelocity.y, moveDirection.z * speed);
        }*/
        if (!InInteraction) {
            if (_walkAction.IsPressed()) {
                Vector2 input = _walkAction.ReadValue<Vector2>();
                Vector3 moveDirection = _cameraTransform.forward * input.y + _cameraTransform.right * input.x;
                moveDirection.y = 0;
                float effectiveAccel = _grounded ? _acceleration : _acceleration / 2;
                _rb.AddForce(moveDirection.normalized * (effectiveAccel * Time.fixedDeltaTime), ForceMode.Force);
                Vector3 movement = _rb.linearVelocity;
                movement.y = 0;
                if (movement.sqrMagnitude > _maxSpeed * _maxSpeed) {
                    movement = movement.normalized * _maxSpeed;
                }

                _rb.linearVelocity = new Vector3(movement.x, _rb.linearVelocity.y, movement.z);
            }
            else {
                Vector2 movement = new(_rb.linearVelocity.x, _rb.linearVelocity.z);
                movement = movement / (1 + _deceleration * Time.fixedDeltaTime);
                _rb.linearVelocity = new(movement.x, _rb.linearVelocity.y, movement.y);
            }
        }
        else _rb.linearVelocity = Vector3.zero;
        RaycastHit hit;
        _grounded = Physics.SphereCast(transform.position, 0.3f, Vector3.down, out hit, _distanceToGround) && !_inJump;
        if (_grounded) {
            // detect if you're on a slope too steep to walk on
            float angle = Vector3.Angle(Vector3.up, hit.normal);
            if (angle > 60) {
                _grounded = false;
                Vector3 normal = hit.normal;
                normal.y = -normal.y;
                _rb.AddForce(normal * (_acceleration *  Time.fixedDeltaTime * (1 - angle/180)), ForceMode.Force);
                return;
            }
            Physics.gravity = Vector3.up * _standardGravity;
            float relativeVeloctiy = Vector3.Dot(Vector3.down, _rb.linearVelocity);
            float x = hit.distance - _distanceToGround;
            float springForce = (x * _springStrength) - (relativeVeloctiy * _springDamper);
            _rb.AddForce(Vector3.down * springForce);
        } else if (_rb.linearVelocity.y < 0) {
            //_rb.mass = _mass * 2;
        }
    }

    private IEnumerator _increaseGravityInJump() {
        for (float t = 2; true; t += Time.deltaTime) {
            if (_grounded) break;
            float newGrav = -t * t * t;
            if (newGrav < _maxGrav) newGrav = _maxGrav;
            Physics.gravity = Vector3.up * newGrav;
            if (_rb.linearVelocity.y < 0) _inJump = false;
            yield return null;
        }
    }

    private void Update() {
        if (InInteraction) return;
        bool jumpPressed = _jumpAction.WasPressedThisFrame();
        if (jumpPressed && _grounded){
            _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z);
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            _inJump = true;
            _grounded = false;
            StartCoroutine(_increaseGravityInJump());
        }
    }

    public void TakeAwayControl() {
        InInteraction = true;
        _camera.Disable();
    }

    public void GiveBackControl() {
        InInteraction = false;
        _camera.Enable();
    }
}