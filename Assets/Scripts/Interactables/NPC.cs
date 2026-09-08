using System;
using UnityEngine;

public class NPC : Interactable {
    public string Name;
    [SerializeField] private GameObject _questSymbol;

    public override void TryInteract() {
        throw new NotImplementedException("TODO");
    }
}
