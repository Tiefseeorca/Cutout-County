using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour {
    public static QuestManager Instance;
    private List<Quest> _activeQuests;
    private List<Quest> _completedQuests;
    private List<Quest> _inactiveQuests;

    public void ActivateQuest(string questId) {
        throw new NotImplementedException("TODO");
    }

    public void CompleteQuest(string questId) {
        throw new NotImplementedException("TODO");
    }

    public void QuestProgressUpdated(string questId) {
        throw new NotImplementedException("TODO");
    }

    public List<string> GetActiveQuests() {
        throw new NotImplementedException("TODO");
    }
}
