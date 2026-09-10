using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour {
    public static QuestManager Instance;
    private List<Quest> _activeQuests = new();
    private List<Quest> _completedQuests = new();
    [SerializeField] private List<Quest> _inactiveQuests;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        List<Quest> reinstantiatedList = new();
        foreach (Quest quest in _inactiveQuests) {
            Quest newQuest = Instantiate(quest);
            reinstantiatedList.Add(newQuest);
            newQuest.AddFlagListener();
        }

        _inactiveQuests = reinstantiatedList;
        //foreach(Quest quest in _inactiveQuests) quest.AddFlagListener();
    }

    public void ActivateQuest(string questId) {
        foreach (Quest quest in _inactiveQuests) {
            if (quest.Id == questId) {
                _activeQuests.Add(quest);
                _inactiveQuests.Remove(quest);
                QuestProgressUpdated();
                return;
            }
        }
    }

    public void CompleteQuest(string questId) {
        foreach (Quest quest in _activeQuests) {
            if (quest.Id == questId) {
                _completedQuests.Add(quest);
                _activeQuests.Remove(quest);
                QuestProgressUpdated();
                return;
            }
        }
    }

    public void QuestProgressUpdated() {
        UIManager.Instance.displayQuests(GetActiveQuestDescriptions());
    }

    public List<string> GetActiveQuestDescriptions() {
        List<string> descs = new();
        foreach (Quest quest in _activeQuests) {
            descs.Add(quest.GetDisplayText());
        }
        return descs;
    }
}
