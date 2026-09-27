using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Author: Timo Lauterbach
/// </summary>
public class GameManager : MonoBehaviour {
	public static GameManager Instance;
	public static UnityEvent GameStarted = new();
	//public static UnityEvent<Flag> FlagChanged = new();
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
	
	public int GetMatchingFlagAmount(Flag[] flags) {
		int counter = 0;
		foreach (Flag flag in flags) {
			if (GetFlagValue(flag.Id) == flag.Value) counter++;
		}
		return counter;
	}

	public bool DoFlagsMatch(Flag[] flags) {
		foreach (Flag flag in flags) {
			if (GetFlagValue(flag.Id) != flag.Value) return false;
		}
		return true;
	}

	private void Start() {
		_gameFlags = Instantiate(_gameFlags);
		StartCoroutine(_startGame());
	}

	private void _doDialogue(Dialogue dialogue) {
		UIManager.Instance.PlayDialogue(dialogue.TextBoxes);
	}

	private IEnumerator _startGame() {
		for (int i = 0; i < 2; i++) {
			yield return null;
		}
		AudioManager.Instance.PlayMusic(AudioIDs.Ambience);
		GameStarted.Invoke();
	}
}
