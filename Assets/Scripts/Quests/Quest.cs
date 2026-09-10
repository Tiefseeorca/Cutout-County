using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Scriptable Objects/Quest")]
public class Quest : ScriptableObject {
    public string Id;
    [SerializeField] private bool _active;
    [SerializeField] private bool _completed;
    [SerializeField] private string _text;
    public Flag[] FlagsToActivate;
    public Flag[] FlagsToComplete;

    public void AddFlagListener() {
        GameFlags.FlagChanged.AddListener(_checkFlags);
    }

    public string GetDisplayText() {
        string res = _text;
        if (FlagsToComplete.Length > 1) {
            res += $" ({_getMatchingFlagAmount(FlagsToComplete)}/{FlagsToComplete.Length})";
        }
        return res;
    }

    private void _activate() {
        _active = true;
        QuestManager.Instance.ActivateQuest(Id);
    }

    private void _complete() {
        _active = false;
        _completed = true;
        GameFlags.FlagChanged.RemoveListener(_checkFlags);
        QuestManager.Instance.CompleteQuest(Id);
    }

    private int _getMatchingFlagAmount(Flag[] flags) {
        int counter = 0;
        foreach (Flag flag in flags) {
            if (GameManager.Instance.GetFlagValue(flag.Id) == flag.Value) counter++;
        }
        return counter;
    }

    private bool _doFlagsMatch(Flag[] flags) {
        foreach (Flag flag in flags) {
            if (GameManager.Instance.GetFlagValue(flag.Id) != flag.Value) return false;
        }
        return true;
    }

    private void _checkFlags(Flag changedFlag) {
        if (_active) {
            // The quest is active, checking if it should be completed
            if(_doFlagsMatch(FlagsToComplete)) {
                _complete();
                return;
            }
            QuestManager.Instance.QuestProgressUpdated();
        } else if (!_completed) {
            // The quest is neither active nor completed, checking if it should be activated
            if (_doFlagsMatch(FlagsToActivate)) _activate();
        }
    }
    
}
