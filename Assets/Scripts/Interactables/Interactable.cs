using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Interactable : MonoBehaviour {
    [SerializeField] protected float _interactionRange;
    [SerializeField] protected InputAction _interactAction;
    public bool blocksOtherInteractions;
    [SerializeField] private MeshRenderer _outlineModel;

    private void OnEnable(){
        _interactAction.Enable();
    }

    private void OnDisable(){
        _interactAction.Disable();
    }

    private void _showOutline() {
        _outlineModel.enabled = true;
    }

    private void _hideOutline() {
        _outlineModel.enabled = false;
    }

    protected virtual void Update() {
        bool interactable = _checkForInteractable();
        if (interactable) {
            _showOutline();
            if (_interactAction.WasPressedThisFrame()) {
                TryInteract();
            }
        } else {
            _hideOutline();
        }
    }

    protected bool _checkForInteractable() {
        Camera mainCam = Camera.main;
        if (mainCam == null) return false;
        if (Vector3.Distance(transform.position, mainCam.transform.position) > _interactionRange || PlayerController.Instance.InInteraction) {
            return false;
        }

        Ray castRay = new Ray(mainCam.transform.position, mainCam.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(castRay, out hit, _interactionRange)){
            return hit.transform == transform;
        }
        return false;
    }
    
    
    public abstract void TryInteract();
}