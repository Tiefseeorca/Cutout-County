using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

/// <summary>
/// Author: Timo Lauterbach
/// </summary>
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
        _torque = new Vector3(Random.value, 0, Random.value).normalized * _spinSpeed;
        _despawnHeight = transform.position.y;
        transform.Rotate(transform.forward, Random.Range(-10f, 10f));
        transform.Rotate(0, Random.Range(-100, 100), Random.Range(-10f, 10f));
    }

    /// <summary>Resets the can back to standard values and rotation for the potential use in ObjectPooling.</summary>
    public void ResetCan() {
        _timesHit = 0;
        transform.rotation = Quaternion.identity;
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        _rb.linearDamping = 0;
    }

    private void _applySpin() {
        _rb.AddTorque(_torque);
    }
    
    public void OnHit() {
        _timesHit++;
        CanGotHit.Invoke(_timesHit);
        _rb.AddForce(Vector3.down * _hitForce, ForceMode.Impulse);
        _applySpin();
    }

    public void LaunchWithForce(Vector3 force) {
        _rb.AddForce(force, ForceMode.Impulse);
    }

    private void Update() {
        if (transform.position.y < _despawnHeight) _despawn();
        else _rb.linearDamping += Time.deltaTime * 3.5f;
    }

    private void _despawn() {
        Destroy(gameObject);
    }
}
