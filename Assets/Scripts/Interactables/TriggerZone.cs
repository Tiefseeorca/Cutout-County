using System;
using UnityEngine;
using UnityEngine.Events;

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
