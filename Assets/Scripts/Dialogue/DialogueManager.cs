using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DialogueManager : MonoBehaviour {
    [Serializable]
    private struct DictItem {
        public string NPC_ID;
        public Dialogue[] Dialogues;
    }

    [Serializable]
    private class NewDict {
        public DictItem[] DictItems;

        public Dictionary<string, Dialogue[]> ToDict() {
            Dictionary<string, Dialogue[]> dict = new();
            foreach (DictItem item in DictItems) {
                Dialogue[] reInstantiatedDialogues = new Dialogue[item.Dialogues.Length];
                for (int i = 0; i < reInstantiatedDialogues.Length; i++) {
                    reInstantiatedDialogues[i] = Instantiate(item.Dialogues[i]);
                }
                dict.Add(item.NPC_ID, reInstantiatedDialogues);
            }
            return dict;
        }
    }
    
    public static DialogueManager Instance;
    [SerializeField] private NewDict _dialogues; 
    private Dictionary<string, Dialogue[]> _dialoguesDict;
    private Flag[] _flagsToSet;

    private void Awake() {
        Instance = this;
        _dialoguesDict = _dialogues.ToDict();
    }

    private void _setFlagsAtDialogueEnd() {
        foreach (Flag flag in _flagsToSet) {
            GameManager.Instance.SetFlagValue(flag.Id, flag.Value);
        }
        _flagsToSet = null;
        Dialogue.DialogueFinished.RemoveListener(_setFlagsAtDialogueEnd);
    }

    public Dialogue GetNextDialogue(string npcId) {
        Dialogue[] dialogues;
        if (_dialoguesDict.TryGetValue(npcId, out dialogues)) {
            List<Dialogue> possibleDialogues = new();
            foreach (Dialogue dialogue in dialogues) {
                if(dialogue.IsAvailable()) possibleDialogues.Add(dialogue);
            }
            Dialogue chosenDialogue;
            // The first dialogue in the list is always exhaust dialogue, which will be played when nothing else is available
            if (possibleDialogues.Count > 1) {
                chosenDialogue = possibleDialogues[Random.Range(1, possibleDialogues.Count)];
            } else {
                chosenDialogue = possibleDialogues[0];
            }
            chosenDialogue.Seen = true;
            _flagsToSet = chosenDialogue.SetFlags;
            Dialogue.DialogueFinished.AddListener(_setFlagsAtDialogueEnd);
            return chosenDialogue;
        } else {
            throw new ArgumentException($"NPC {npcId} is does not have dialogue or does not exist");
        }
    }
}
