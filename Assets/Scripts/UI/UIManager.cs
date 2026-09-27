using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// <p>The UIManager has access to all relevant UI elements and handles activation and deactivation of them.<br/>
///     Can be requested to play Dialogue and display active/completed quests.<br/>
///     This Manager is used for both in game and in the main menu.</p>
/// <p>Author: Timo Lauterbach & Burak Namazci</p>
/// </summary>
public class UIManager : MonoBehaviour {
    public static UIManager Instance;
    [SerializeField] private GameObject _pauseScreen;
    [SerializeField] private GameObject _optionsScreen;
    [SerializeField] private GameObject _dialogueBox;
    private TextMeshProUGUI _dialogueBoxText;
    private DialogueHelper _dialogueHelper;
    private InputAction _dialogueContinueAction;
    [SerializeField] private TextMeshProUGUI _questsDisplay;
    [SerializeField] private TextMeshProUGUI _completedQuestsDisplay;
    [SerializeField] private GameObject _minigameEndscreen;
    [SerializeField] private TextMeshProUGUI _minigameFinalScoreDisplay;
    [SerializeField] private TextMeshProUGUI _minigameHighscoreDisplay;
    [SerializeField] private GameObject _minigameUI;
    [SerializeField] private TextMeshProUGUI _minigameScoreDisplay;
    private bool _isPaused = false;
    private CursorLockMode _unpausedLockMode;

    // Timo Lauterbach
    private void Awake(){
        Instance = this;

        if (_dialogueBox != null){
            _dialogueBoxText = _dialogueBox.GetComponentInChildren<TextMeshProUGUI>();
        }

        _dialogueContinueAction = InputSystem.actions.FindAction("Jump");
    }

    // Burak Namazci
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

    // Timo Lauterbach
    public void displayQuests(List<string> questTexts) {
        _questsDisplay.text = _listToLines(questTexts);
    }

    // Timo Lauterbach
    public void displayFinishedQuests(List<string> questTexts) {
        _completedQuestsDisplay.text = _listToLines(questTexts);
    }

    /// <summary>
    /// <p>Takes all dialogue to display and starts a Coroutine that displays the dialogue correctly</p>
    /// <p>Author: Timo Lauterbach</p>
    /// </summary>
    /// <param name="dialogueBoxes">A list of the texts for all dialogue boxes to display</param>
    public void PlayDialogue(List<string> dialogueBoxes) {
        _dialogueBox.SetActive(true);
        _dialogueHelper = new DialogueHelper(dialogueBoxes, _displaySingleDialogueText);
        StartCoroutine(_listenForDialogueContinueInput());
    }

    // Timo Lauterbach
    private void _displaySingleDialogueText(string text) {
        _dialogueBoxText.text = text;
    }

    // Timo Lauterbach
    private IEnumerator _listenForDialogueContinueInput() {
        bool inDialogue = true;
        while (inDialogue) {
            inDialogue = _dialogueHelper.DisplayNextText();
            yield return new WaitForSeconds(0.1f);
            yield return new WaitUntil(() => _dialogueContinueAction.WasPressedThisFrame());
        }
        _dialogueHelper = null;
        _dialogueBox.SetActive(false);
        Dialogue.DialogueFinished.Invoke();
    }

    // Timo Lauterbach
    public void ShowMinigameUI() {
        _minigameUI.SetActive(true);
        _questsDisplay.gameObject.SetActive(false);
    }

    // Timo Lauterbach
    public void UpdateMinigameScore(int score) {
        _minigameScoreDisplay.text = score.ToString();
    }
    
    // Timo Lauterbach
    private IEnumerator _hideMinigameEndscreenAfterSeconds(float t, Action closeFunction) {
        yield return new WaitForSeconds(t);
        closeFunction.Invoke();
        _minigameEndscreen.SetActive(false);
        _questsDisplay.gameObject.SetActive(true);
    }

    // Timo Lauterbach
    public void DisplayMinigameEndscreen(Minigame minigame) {
        _minigameEndscreen.SetActive(true);
        _minigameFinalScoreDisplay.text = minigame.Score.ToString();
        _minigameHighscoreDisplay.text = minigame.Highscore.ToString();
        _minigameUI.SetActive(false);
        StartCoroutine(_hideMinigameEndscreenAfterSeconds(Minigame.MinigameEndScreenDuration, minigame.EndMinigame));
    }
    
    // On Button / Key Press

    // Burak Namazci
    public void Pause() {
        _unpausedLockMode = Cursor.lockState;
        Cursor.lockState = CursorLockMode.None;
        _pauseScreen.SetActive(true);
        Time.timeScale = 0;
        _isPaused = true;
        AudioManager.Instance.PauseCurrentMusic();
    }

    // Burak Namazci
    public void Resume(){
        Cursor.lockState = _unpausedLockMode;
        _pauseScreen.SetActive(false);
        Time.timeScale = 1;
        _isPaused = false;
        AudioManager.Instance.ResumeCurrentMusic();
    }

    public void OpenOptions() {
        throw new NotImplementedException("TODO");
    }

    public void CloseOptions() {
        throw new NotImplementedException("TODO");
    }

    // Burak Namazci
    public void BackToMenu(){
        Time.timeScale = 1f;
        _loadScene("MainMenu");
    }
    
    // Main Menu
    
    // Burak Namazci
    public void StartGame(){
        _loadScene("LEVEL");
    }
    
    // Burak Namazci
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

    // Timo Lauterbach
    private void _loadScene(string sceneName){
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// <p>Converts a list of strings into a single string with each entry separated by a new line.</p>
    /// <p>Author: Timo Lauterbach</p>
    /// </summary>
    /// <param name="texts">A list of any strings</param>
    /// <returns>All strings from the list in one string, separated by a new line</returns>
    private string _listToLines(List<string> texts) {
        StringBuilder sb = new();
        foreach (string text in texts) {
            sb.Append(text);
            sb.Append("\n");
        }
        return sb.ToString();
    }
}