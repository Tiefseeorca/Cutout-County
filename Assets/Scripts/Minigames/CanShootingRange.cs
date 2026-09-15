using System;
using UnityEngine;

public class CanShootingRange : Minigame {
	[SerializeField] private GameObject _canPrefab;
	[SerializeField] private float _startCanCooldown;
	[SerializeField] private float _minCanCooldown;
	[SerializeField] private float _gameDuration;
	[SerializeField] private float _canLaunchForce;
	private float _gameRuntime;
	private float _cooldownTimer;

	private void Start() {
		this.StartMinigame();
	}

	/// <summary>Spawns a can on a random position along the local x-axis with a random angle that keeps the can inside the game space.</summary>
	private void _spawnCan() {
		// TODO: include the randomness
		GameObject canObject = Instantiate(_canPrefab, transform.position, Quaternion.identity);
		Can can = canObject.GetComponent<Can>();
		can.LaunchWithForce(Vector3.up * _canLaunchForce);
	}

	/// <summary>Sets the cooldown for the next can to spawn. Speeds up linearly with game progression.</summary>
	private void _setCooldown() {
		float rel = _gameRuntime / _gameDuration;
		_cooldownTimer = (1 - rel) * _startCanCooldown + rel * _minCanCooldown;
	}

	protected override void _resetGame() {
		_gameRuntime = 0;
		_cooldownTimer = _startDelay;
	}
	protected override void _executeGame() {
		_cooldownTimer -= Time.deltaTime;
		_gameRuntime += Time.deltaTime;
		if (_gameRuntime >= _gameDuration) {
			_ingame = false;
			return;
		}
		if (_cooldownTimer < 0) {
			_spawnCan();
			_setCooldown();
		}
		// TODO: Implement click recognition here or in Can via OnMouseOver
	}
	protected override void _finishOffGame() {
		Debug.LogWarning("TODO: Implement score display of minigame");
		EndMinigame();
	}
}
