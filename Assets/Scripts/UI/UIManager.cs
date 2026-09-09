using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour {
    public static UIManager Instance;
    [SerializeField] private GameObject _pauseScreen;
    [SerializeField] private GameObject _optionsScreen;
    [SerializeField] private GameObject _dialogueBox;
    [SerializeField] private TextMeshProUGUI _questsDisplay;

    private void Awake() {
        Instance = this;
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

    public void playDialogue(List<string> dialogueBoxes) {
        throw new NotImplementedException("TODO");
    }

    public void _displaySingleDialogueText(string text) {
        throw new NotImplementedException("TODO");
    }
    
    // On Button Press

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
