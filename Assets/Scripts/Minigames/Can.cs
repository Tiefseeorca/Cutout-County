using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class Can : MonoBehaviour {
    /// <summary>Gets invoked when a can got hit. Passes the amount of times that can has already been hit</summary>
    public static UnityEvent<int> CanGotHit = new();
    private int _timesHit = 0;
    [SerializeField] private float _spinSpeed;
    [SerializeField] private float _hitForce;
    private Rigidbody _rb;
    private Vector3 _torque;
    private float _despawnHeight;

    private void Awake() {
        _rb = GetComponent<Rigidbody>();
        _torque = new Vector3(Random.value, Random.value, Random.value).normalized * _spinSpeed;
        _despawnHeight = transform.position.y;
        _applySpin();
    }

    /// <summary>Resets the can back to standard values and rotation for the use in ObjectPooling.</summary>
    public void ResetCan() {
        _timesHit = 0;
        transform.rotation = Quaternion.identity;
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
    }

    private void _applySpin() {
        _rb.AddTorque(_torque);
    }
    
    public void OnHit() {
        _timesHit++;
        CanGotHit.Invoke(_timesHit);
    }

    public void LaunchWithForce(Vector3 force) {
        _rb.AddForce(force, ForceMode.Impulse);
    }

    private void Update() {
        if (transform.position.y < _despawnHeight) _despawn();
    }

    private void _despawn() {
        Destroy(gameObject);
    }

    private void OnMouseOver() {
        // TODO: Implement click recognition via this or in CanShootingRange
    }
}
