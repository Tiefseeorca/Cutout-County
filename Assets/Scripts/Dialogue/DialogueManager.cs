using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour {
    public DialogueManager Instance;
    private Dictionary<string, List<Dialogue>> _dialogues;

    private void Awake() {
        Instance = this;
    }

    public Dialogue GetNextDialogue(string dialogueId) {
        throw new NotImplementedException("TODO");
    }
}
