using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Author: Timo Lauterbach & Burak Namazci
/// </summary>
public abstract class Interactable : MonoBehaviour {
    [SerializeField] protected float _interactionRange;
    [SerializeField] protected InputAction _interactAction;
    public bool blocksOtherInteractions;
    [SerializeField] private MeshRenderer _outlineModel;
    [SerializeField] private Flag[] _flagsToDeactivate;

    // Burak Namazci
    private void OnEnable(){
        _interactAction.Enable();
    }

    // Burak Namazci
    private void OnDisable(){
        _interactAction.Disable();
    }

    // Timo Lauterbach
    private void Awake() {
        GameFlags.FlagChanged.AddListener(_checkFlags);
    }

    /// <summary>
    /// <p>Checks if this Interactable should deactivate itself when a flag is changed.</p>
    /// <p>Author: Timo Lauterbach</p>
    /// </summary>
    /// <param name="changedFlag">The flag that just got changed, passed by the event</param>
    private void _checkFlags(Flag changedFlag) {
        if (_flagsToDeactivate != null && _flagsToDeactivate.Length == 0) return;
        foreach (Flag flag in _flagsToDeactivate) {
            if (GameManager.Instance.GetFlagValue(flag.Id) != flag.Value) return;
        }
        GameFlags.FlagChanged.RemoveListener(_checkFlags);
        gameObject.SetActive(false);
    }

    // Timo Lauterbach
    private void _showOutline() {
        _outlineModel.enabled = true;
    }

    // Timo Lauterbach
    private void _hideOutline() {
        _outlineModel.enabled = false;
    }

    // Timo Lauterbach & Burak Namazci
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

    /// <summary>
    /// <p>Checks if the player is close enough and looking at the Interactable</p>
    /// <p>Author: Burak Namazci</p>
    /// </summary>
    /// <returns>true if the Interactable can be interacted with, otherwise false</returns>
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
    
    // Timo Lauterbach
    public abstract void TryInteract();
}