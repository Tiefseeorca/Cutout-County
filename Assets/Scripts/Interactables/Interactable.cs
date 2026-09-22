using System;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Interactable : MonoBehaviour {
    [SerializeField] protected float _interactionRange;
    [SerializeField] protected InputAction _interactAction;
    public bool blocksOtherInteractions;
    [SerializeField] private MeshRenderer _outlineModel;
    [SerializeField] private Flag[] _flagsToDeactivate;

    private void OnEnable(){
        _interactAction.Enable();
    }

    private void OnDisable(){
        _interactAction.Disable();
    }

    private void Awake() {
        GameFlags.FlagChanged.AddListener(_checkFlags);
    }

    private void _checkFlags(Flag changedFlag) {
        if (_flagsToDeactivate != null && _flagsToDeactivate.Length == 0) return;
        foreach (Flag flag in _flagsToDeactivate) {
            if (GameManager.Instance.GetFlagValue(flag.Id) != flag.Value) return;
        }
        GameFlags.FlagChanged.RemoveListener(_checkFlags);
        gameObject.SetActive(false);
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
        if (Physics.Raycast(castRay, out hit, _interactionRange, LayerMask.GetMask("Interactables"))) {
            return hit.transform == transform;
        }
        return false;
    }
    
    
    public abstract void TryInteract();
}