using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour {
	public static GameManager Instance;
	public static UnityEvent GameStarted = new();
	[SerializeField] private GameFlags _gameFlags;

	public bool IsPaused;

	private void Awake() {
		Instance = this;
	}

	public void PauseGame() {
		throw new NotImplementedException("TODO");
	}

	public void ResumeGame() {
		throw new NotImplementedException("TODO");
	}

	public bool GetFlagValue(string flagId) {
		return _gameFlags.GetFlagById(flagId).Value;
	}

	public void SetFlagValue(string flagId, bool value) {
		_gameFlags.SetGameFlag(flagId, value);
	}

	private void Start() {
		_gameFlags = Instantiate(_gameFlags);
		StartCoroutine(_startGame());
	}

	private IEnumerator _startGame() {
		for (int i = 0; i < 1; i++) {
			yield return null;
		}
		GameStarted.Invoke();
	}
}
