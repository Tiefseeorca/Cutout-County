using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour {
    public static UIManager Instance;
    [SerializeField] private GameObject _pauseScreen;
    [SerializeField] private GameObject _optionsScreen;
    [SerializeField] private GameObject _dialogueBox;
    private TextMeshProUGUI _dialogueBoxText;
    private DialogueHelper _dialogueHelper;
    private InputAction _dialogueContinueAction;
    [SerializeField] private TextMeshProUGUI _questsDisplay;

    private void Awake() {
        Instance = this;
        _dialogueBoxText = _dialogueBox.GetComponentInChildren<TextMeshProUGUI>();
        _dialogueContinueAction = InputSystem.actions.FindAction("Jump");
    }

    // In Gameplay

    public void displayQuests(List<string> questTexts) {
        StringBuilder displayText = new StringBuilder();
        foreach (string line in questTexts) {
            displayText.Append(line);
            displayText.Append("\n");
        }
        _questsDisplay.text = displayText.ToString();
    }

    public void PlayDialogue(List<string> dialogueBoxes) {
        _dialogueBox.SetActive(true);
        _dialogueHelper = new DialogueHelper(dialogueBoxes, _displaySingleDialogueText);
        StartCoroutine(_listenForDialogueContinueInput());
    }

    private void _displaySingleDialogueText(string text) {
        _dialogueBoxText.text = text;
    }

    private IEnumerator _listenForDialogueContinueInput() {
        bool inDialogue = true;
        while (inDialogue) {
            inDialogue = _dialogueHelper.DisplayNextText();
            yield return new WaitUntil(() => _dialogueContinueAction.IsPressed());
        }
        _dialogueHelper = null;
        _dialogueBox.SetActive(false);
        Dialogue.DialogueFinished.Invoke();
    }
    
    // On Button / Key Press

    public void Pause() {
        throw new NotImplementedException("TODO");
    }

    public void Resume() {
        throw new NotImplementedException("TODO");
    }

    public void OpenOptions() {
        throw new NotImplementedException("TODO");
    }

    public void CloseOptions() {
        throw new NotImplementedException("TODO");
    }

    public void BackToMenu() {
        throw new NotImplementedException("TODO");
    }
    
    // Main Menu

    public void StartGame() {
        throw new NotImplementedException("TODO");
    }

    public void QuitGame() {
        throw new NotImplementedException("TODO");
    }
    
    // Options

    public void AdjustMasterVolume(float value) {
        throw new NotImplementedException("TODO");
    }

    public void AdjustMusicVolume(float value) {
        throw new NotImplementedException("TODO");
    }

    public void AdjustSfxVolume(float value) {
        throw new NotImplementedException("TODO");
    }
    
    // Helpers

    private void _loadScene(string sceneName) {
        throw new NotImplementedException("TODO");
    }
}
