using System;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour {
	public static GameManager Instance;
	public static UnityEvent GameStarted = new();
	[SerializeField] private GameFlags _gameFlags;

	public bool IsPaused;

	public void PauseGame() {
		throw new NotImplementedException("TODO");
	}

	public void ResumeGame() {
		throw new NotImplementedException("TODO");
	}

	public bool GetFlagValue(string flagId) {
		return _gameFlags.GetFlagById(flagId).Value;
	}

	private void Start() {
		_gameFlags = Instantiate(_gameFlags);
	}
}
