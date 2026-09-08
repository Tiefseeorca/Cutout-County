using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueHelper {
    private List<string> _texts;
    private int _currentPointer;
    [SerializeField] private InputAction _continueAction;

    public DialogueHelper(List<string> texts) {
        _texts = texts;
        throw new NotImplementedException("TODO");
    }
    
    private void _displayNextText() {
        throw new NotImplementedException("TODO");
    }
}
