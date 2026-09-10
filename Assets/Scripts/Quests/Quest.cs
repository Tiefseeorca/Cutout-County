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
            res += $" ({GameManager.Instance.GetMatchingFlagAmount(FlagsToComplete)}/{FlagsToComplete.Length})";
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

    private void _checkFlags(Flag changedFlag) {
        if (_active) {
            // The quest is active, checking if it should be completed
            if(GameManager.Instance.DoFlagsMatch(FlagsToComplete)) {
                _complete();
                return;
            }
            QuestManager.Instance.QuestProgressUpdated();
        } else if (!_completed) {
            // The quest is neither active nor completed, checking if it should be activated
            if (GameManager.Instance.DoFlagsMatch(FlagsToActivate)) _activate();
        }
    }
    
}
