using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Author: Timo Lauterbach
/// </summary>
public class DialogueHelper {
    private List<string> _texts;
    private int _currentPointer;
    private Action<string> _displayFunction;

    public DialogueHelper(List<string> texts, Action<string> displayFunction) {
        _texts = texts;
        _displayFunction = displayFunction;
    }
    
    public bool DisplayNextText() {
        if (_currentPointer >= _texts.Count) return false;
        _displayFunction(_texts[_currentPointer++]);
        return _currentPointer < _texts.Count;
    }
}
