using UnityEngine;

/// <summary>
/// Author: Timo Lauterbach
/// </summary>
public class MinigameInteractable : Interactable {
	[SerializeField] private Minigame _minigame;
	
	public override void TryInteract() {
		if (PlayerController.Instance.InInteraction) return;
		_minigame.StartMinigame();
	}
}
