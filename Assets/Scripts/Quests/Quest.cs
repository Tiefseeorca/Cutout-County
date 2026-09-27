using System;
using UnityEngine;

/// <summary>
/// Author: Timo Lauterbach
/// </summary>
[CreateAssetMenu(fileName = "Quest", menuName = "Scriptable Objects/Quest")]
public class Quest : ScriptableObject {
    public string Id;
    [SerializeField] private bool _active;
    [SerializeField] private bool _completed;
    [SerializeField] private string _text;
    [SerializeField] private string _completionText;
    public Flag[] FlagsToActivate;
    public Flag[] FlagsToComplete;

    // gets called by the QuestManager upon start of the game, after the object has been re-instantiated to avoid changing it in editor.
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

    public string GetCompletionText() {
        return _completionText;
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

    /// <summary>
    /// Checks if the required flags for activation or completion are met. Checks twice upon activation for the case of instant completion.
    /// </summary>
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
            if (GameManager.Instance.DoFlagsMatch(FlagsToActivate)) {
                _activate();
                _checkFlags(changedFlag);
            }
        }
    }
    
}
