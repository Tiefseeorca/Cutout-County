using System;
using UnityEngine;
using UnityEngine.Events;

public class GoldCoin : Interactable {
    public static UnityEvent<string> CoinCollected;
    [SerializeField] private string _flagId;
    private bool _isCollected;

    private void _collect() {
        throw new NotImplementedException("TODO");
    }

    public override void TryInteract() {
        
    }
}
