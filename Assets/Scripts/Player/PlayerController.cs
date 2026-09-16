using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour {
    public static PlayerController Instance;
    public float speed;
    public float jumpForce;
    [SerializeField] private float _distanceToGround;
    [SerializeField] private float _springStrength;
    [SerializeField] private float _springDamper;
    [SerializeField] private float _maxMass;

    public bool InInteraction;
    [SerializeField] private bool _grounded;
    [SerializeField] private bool _inJump;
    private float _mass;
    
    private Rigidbody _rb;
    private InputAction _walkAction;
    private InputAction _jumpAction;
    private Transform _cameraTransform;
    private POVCameraHelper _camera;
    
    void Awake() {
        Cursor.lockState = CursorLockMode.Locked;
        Instance = this;
        _rb = GetComponent<Rigidbody>();
        _mass = _rb.mass;
        _walkAction = InputSystem.actions.FindAction("Move");
        _jumpAction = InputSystem.actions.FindAction("Jump");
        _cameraTransform = transform.GetChild(0).GetChild(0);
        _camera = _cameraTransform.GetComponent<POVCameraHelper>();
    }

    void FixedUpdate(){
        if (!InInteraction) {
            Vector2 input = _walkAction.ReadValue<Vector2>();
            Vector3 moveDirection = _cameraTransform.forward * input.y + _cameraTransform.right * input.x;
            _rb.linearVelocity = new Vector3(moveDirection.x * speed, _rb.linearVelocity.y, moveDirection.z * speed);
        }
        RaycastHit hit;
        _grounded = Physics.Raycast(transform.position, Vector3.down, out hit, _distanceToGround) && !_inJump;
        if (_grounded) {
            _rb.mass = _mass;
            float relativeVeloctiy = Vector3.Dot(Vector3.down, _rb.linearVelocity);
            float x = hit.distance - _distanceToGround;
            float springForce = (x * _springStrength) - (relativeVeloctiy * _springDamper);
            _rb.AddForce(Vector3.down * springForce);
            //_rb.AddForce(Vector3.up * (_springStrength * 1f / Mathf.Max(hit.distance * _springSoftness, 0.01f)));
        } else if (_rb.linearVelocity.y < 0) {
            //_rb.mass = _mass * 2;
        }
    }

    private IEnumerator _increaseGravityInJump() {
        for (float t = 1; true; t += Time.deltaTime * 3) {
            if (_grounded) break;
            float newMass = _mass * t * t;
            if (newMass > _maxMass) newMass = _maxMass;
            _rb.mass = newMass;
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