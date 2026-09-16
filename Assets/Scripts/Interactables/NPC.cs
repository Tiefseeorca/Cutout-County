using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPC : Interactable{
    public string Name;
    [SerializeField] private GameObject _questSymbol;
    //[SerializeField] private InputAction _cancelAction;
    private bool _inConversation;
    
    public override void TryInteract(){
        if (_inConversation || PlayerController.Instance.InInteraction) return;

        _inConversation = true;
        PlayerController.Instance.InInteraction = true;

        if (_questSymbol != null){
            _questSymbol.SetActive(false);
        }
        
        PlayerController.Instance.TakeAwayControl();
        Dialogue nextDialogue = DialogueManager.Instance.GetNextDialogue(Name);
        UIManager.Instance.PlayDialogue(nextDialogue.TextBoxes);
        Dialogue.DialogueFinished.AddListener(_endConversation);
        //Debug.Log("Dialog mit NPC gestartet. Drücke ESC zum beenden");
    }

    private void _endConversation(){
        _inConversation = false;
        blocksOtherInteractions = false;
        
        PlayerController.Instance.GiveBackControl();
        Dialogue.DialogueFinished.RemoveListener(_endConversation);
        //Debug.Log("Dialog mit NPC beendet");
    }
}