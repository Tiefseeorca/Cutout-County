using System;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour {
	public static GameManager Instance;
	public static UnityEvent GameStarted = new();

	public bool IsPaused;

	public void PauseGame() {
		throw new NotImplementedException("TODO");
	}

	public void ResumeGame() {
		throw new NotImplementedException("TODO");
	}
}
