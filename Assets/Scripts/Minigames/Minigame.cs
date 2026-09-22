using System;
using Unity.Cinemachine;
using UnityEngine;

public abstract class Minigame : MonoBehaviour {
    public const float MinigameEndScreenDuration = 4f;
    [SerializeField] private CinemachineCamera _camera;
    protected bool _active;
    protected bool _ingame;
    public int Score { get; protected set; }
    public int Highscore { get; protected set; }
    [SerializeField] protected float _startDelay;
    [SerializeField] protected float _endDelay;
    [SerializeField] private int[] _scoreHurdles;
    [SerializeField] private string _scoreFlagsPrefix;

    /// <summary>Call this to start a minigame. Takes away player control and changes the camera to the minigame camera.</summary>
    public void StartMinigame() {
        PlayerController.Instance.TakeAwayControl();
        CameraManager.Instance.SwitchTo(_camera);
        Cursor.lockState = CursorLockMode.Confined;
        _active = true;
        _ingame = true;
        Score = 0;
        _resetGame();
        UIManager.Instance.ShowMinigameUI();
        UIManager.Instance.UpdateMinigameScore(Score);
    }
    
    /// <summary>Call this to give the player control back and resume the game like normal, stopping the minigame. Use this to either cancel an ongoing minigame or at the end of one.</summary>
    public void EndMinigame() {
        _active = false;
        _ingame = false;
        foreach (int scoreHurdle in _scoreHurdles) {
            if(Score > scoreHurdle) GameManager.Instance.SetFlagValue(_scoreFlagsPrefix + scoreHurdle, true);
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        CameraManager.Instance.SwitchTo(CameraManager.Instance.PlayerCamera);
        PlayerController.Instance.GiveBackControl();
    }

    private void Update() {
        if (!_active) return;
        if (_ingame) _executeGame();
        else _finishOffGame();
    }

    /// <summary>Resets all values unique to the specific minigame.</summary>
    protected abstract void _resetGame();

    /// <summary>Implements the actual minigame functionality.</summary>
    protected abstract void _executeGame();
    
    /// <summary>Implements any kind of score display or other functionality at the end of the game before completing it.</summary>
    protected abstract void _finishOffGame();
}
