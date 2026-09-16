using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public static UIManager Instance;
    [SerializeField] private GameObject _pauseScreen;
    [SerializeField] private GameObject _optionsScreen;
    [SerializeField] private GameObject _dialogueBox;
    private TextMeshProUGUI _dialogueBoxText;
    private DialogueHelper _dialogueHelper;
    private InputAction _dialogueContinueAction;
    [SerializeField] private TextMeshProUGUI _questsDisplay;
    [SerializeField] private GameObject _minigameEndscreen;
    [SerializeField] private TextMeshProUGUI _minigameFinalScoreDisplay;
    [SerializeField] private TextMeshProUGUI _minigameHighscoreDisplay;
    [SerializeField] private GameObject _minigameUI;
    [SerializeField] private TextMeshProUGUI _minigameScoreDisplay;
    private bool _isPaused = false;

    private void Awake(){
        Instance = this;

        if (_dialogueBox != null){
            _dialogueBoxText = _dialogueBox.GetComponentInChildren<TextMeshProUGUI>();
        }

        _dialogueContinueAction = InputSystem.actions.FindAction("Jump");
    }

    private void Update(){
        if (Keyboard.current.escapeKey.wasPressedThisFrame){
            if (_isPaused == true){
                Resume();
            }
            else{
                Pause();
            }
        }
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

    public void ShowMinigameUI() {
        _minigameUI.SetActive(true);
        _questsDisplay.gameObject.SetActive(false);
    }

    public void UpdateMinigameScore(int score) {
        _minigameScoreDisplay.text = score.ToString();
    }
    
    private IEnumerator _hideMinigameEndscreenAfterSeconds(float t, Action closeFunction) {
        yield return new WaitForSeconds(t);
        closeFunction.Invoke();
        _minigameEndscreen.SetActive(false);
        _questsDisplay.gameObject.SetActive(true);
    }

    public void DisplayMinigameEndscreen(Minigame minigame) {
        _minigameEndscreen.SetActive(true);
        _minigameFinalScoreDisplay.text = minigame.Score.ToString();
        _minigameHighscoreDisplay.text = minigame.Highscore.ToString();
        _minigameUI.SetActive(false);
        StartCoroutine(_hideMinigameEndscreenAfterSeconds(Minigame.MinigameEndScreenDuration, minigame.EndMinigame));
    }
    
    // On Button / Key Press

    public void Pause(){
        Cursor.lockState = CursorLockMode.None;
        _pauseScreen.SetActive(true);
        Time.timeScale = 0;
        _isPaused = true;
    }

    public void Resume(){
        Cursor.lockState = CursorLockMode.Locked;
        _pauseScreen.SetActive(false);
        Time.timeScale = 1;
        _isPaused = false;
    }

    public void OpenOptions() {
        throw new NotImplementedException("TODO");
    }

    public void CloseOptions() {
        throw new NotImplementedException("TODO");
    }

    public void BackToMenu(){
        Time.timeScale = 1f;
        _loadScene("MainMenu");
    }
    
    // Main Menu
    
    public void StartGame(){
        _loadScene("SampleScene");
    }
    
    public void QuitGame(){
        Debug.Log("Quitting game");
        Application.Quit();
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

    private void _loadScene(string sceneName){
        SceneManager.LoadScene(sceneName);
    }
}