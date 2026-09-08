using System;
using UnityEngine;

public class Quest : ScriptableObject {
    public string Id;
    private bool _active;
    private bool _completed;
    private string _text;
    public Flag[] FlagsToActivate;
    public Flag[] FlagsToComplete;

    private void Awake() {
        throw new NotImplementedException("TODO");
    }

    public string GetDisplayText() {
        throw new NotImplementedException("TODO");
    }

    private void _activate() {
        throw new NotImplementedException("TODO");
    }

    private void _complete() {
        throw new NotImplementedException("TODO");
    }

    private void _checkFlags(Flag changedFlag) {
        throw new NotImplementedException("TODO");
    }
    
}
