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

    protected bool _checkForInteractable() {
        Camera mainCam = Camera.main;
        if (mainCam == null) return false;

        Ray castRay = new Ray(mainCam.transform.position, mainCam.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(castRay, out hit, _interactionRange)){
            return hit.transform == transform;
        }
        return false;
    }
    
    
    public abstract void TryInteract();
}