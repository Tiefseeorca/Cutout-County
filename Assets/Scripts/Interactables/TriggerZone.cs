using System;
using UnityEngine;
using UnityEngine.Events;

public class TriggerZone : MonoBehaviour {
    public Flag[] SetFlagsOnEnter;
    public UnityEvent EventOnEnter;
    public Flag[] SetFlagsOnExit;
    public UnityEvent EventOnExit;
    public bool DestroyOnExit;

    private void OnTriggerEnter(Collider other) {
        foreach (Flag flag in SetFlagsOnEnter) {
            GameManager.Instance.SetFlagValue(flag.Id, flag.Value);
        }
        if(EventOnEnter != null) EventOnEnter.Invoke();
    }
    
    private void OnTriggerExit(Collider other) {
        foreach (Flag flag in SetFlagsOnExit) {
            GameManager.Instance.SetFlagValue(flag.Id, flag.Value);
        }
        if(EventOnExit != null) EventOnExit.Invoke();
        if (DestroyOnExit) Destroy(gameObject);
    }
}
