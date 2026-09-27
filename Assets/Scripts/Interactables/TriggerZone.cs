using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// <p>TriggerZones can be used to set any flag or trigger any event upon contact or leaving the area.
///     They can be one time or persistent.<br/>
///     This can be used for testing, debugging or area / distance based events.</p>
/// <p>Author: Timo Lauterbach</p>
/// </summary>
public class TriggerZone : MonoBehaviour {
    public Flag[] SetFlagsOnEnter;
    public UnityEvent EventOnEnter;
    public Flag[] SetFlagsOnExit;
    public UnityEvent EventOnExit;
    public bool DestroyOnEnter;
    public bool DestroyOnExit;

    [SerializeField] private float _gizmoRadius = 0;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        foreach (Flag flag in SetFlagsOnEnter) {
            GameManager.Instance.SetFlagValue(flag.Id, flag.Value);
        }
        if(EventOnEnter != null) EventOnEnter.Invoke();
        if(DestroyOnEnter) Destroy(gameObject);
    }
    
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        foreach (Flag flag in SetFlagsOnExit) {
            GameManager.Instance.SetFlagValue(flag.Id, flag.Value);
        }
        if(EventOnExit != null) EventOnExit.Invoke();
        if (DestroyOnExit) Destroy(gameObject);
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, _gizmoRadius);
    }
}
