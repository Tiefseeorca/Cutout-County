using UnityEngine;

public class MinigameInteractable : Interactable {
	[SerializeField] private Minigame _minigame;
	
	public override void TryInteract() {
		_minigame.StartMinigame();
	}
}
