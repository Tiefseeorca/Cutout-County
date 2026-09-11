using System;
using UnityEngine;
using UnityEngine.Events;

public class GoldCoin : Interactable {
    public static UnityEvent<string> CoinCollected;
    [SerializeField] private string _flagId;
    private bool _isCollected;

    private void _collect(){
        GameManager.Instance.SetFlagValue(_flagId, true);
        CoinCollected?.Invoke(_flagId);
        
        //bool checkValue = GameManager.Instance.GetFlagValue(_flagId);
        //Debug.Log($"Münze {_flagId} eingesammelt. Wert im GameManager ist jezt: {checkValue}");
        Destroy(gameObject);
    }
    
    public override void TryInteract(){
        if (_isCollected) return;
        _isCollected = true;
        _collect();
    }
}