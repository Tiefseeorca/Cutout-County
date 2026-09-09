using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPC : Interactable{
    public string Name;
    [SerializeField] private GameObject _questSymbol;
    private bool _inConversation;
    
    public override void TryInteract(){
        if (_inConversation) return;

        _inConversation = true;
        blocksOtherInteractions = true;

        if (_questSymbol != null){
            _questSymbol.SetActive(false);
        }
        Debug.Log("Dialog mit NPC gestartet. Drücke ESC zum beenden");
    }

    protected override void Update(){
        base.Update();
        
        if (_inConversation){
            if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame){
                _endConversation();
            }
        }
    }

    private void _endConversation(){
        _inConversation = false;
        blocksOtherInteractions = false;
        Debug.Log("Dialog mit NPC beendet");
    }
}