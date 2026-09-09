using System;
using UnityEngine;
using UnityEngine.Events;

public class GoldCoin : Interactable {
    public static UnityEvent<string> CoinCollected;
    [SerializeField] private string _flagId;
    private bool _isCollected;

    private void _collect(){
        CoinCollected?.Invoke(_flagId);
        Destroy(gameObject);
        Debug.Log("Coin collected");
    }
    
    public override void TryInteract(){
        if (_isCollected) return;
        _isCollected = true;
        _collect();
    }
}