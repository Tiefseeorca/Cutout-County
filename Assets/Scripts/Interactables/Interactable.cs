using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Interactable : MonoBehaviour {
    [SerializeField] protected float _interactionRange;
    [SerializeField] protected InputAction _interactAction;
    public bool blocksOtherInteractions;

    protected bool _checkForInteractable() {
        return Vector3.Distance(transform.position, PlayerController.Instance.transform.position) <= _interactionRange;
    }

    public abstract void TryInteract();
}
