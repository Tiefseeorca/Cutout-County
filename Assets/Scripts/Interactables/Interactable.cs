using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Interactable : MonoBehaviour {
    [SerializeField] protected float _interactionRange;
    [SerializeField] protected InputAction _interactAction;
    public bool blocksOtherInteractions;

    private void OnEnable(){
        _interactAction.Enable();
    }

    private void OnDisable(){
        _interactAction.Disable();
    }

    protected virtual void Update(){
        if (_interactAction.WasPressedThisFrame() && _checkForInteractable()){
            TryInteract();
        }
    }
    
    protected bool _checkForInteractable(){
        return Vector3.Distance(transform.position, PlayerController.Instance.transform.position) <= _interactionRange;
    }

    public abstract void TryInteract();
}