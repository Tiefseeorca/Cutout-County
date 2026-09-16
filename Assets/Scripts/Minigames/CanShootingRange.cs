using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class CanShootingRange : Minigame {
	[SerializeField] private GameObject _canPrefab;
	[SerializeField] private GameObject _sparksPrefab;
	[SerializeField] private float _startCanCooldown;
	[SerializeField] private float _minCanCooldown;
	[SerializeField] private float _gameDuration;
	[SerializeField] private float _canLaunchForce;
	[SerializeField] private float _spawnRadius;
	private float _gameRuntime;
	private float _cooldownTimer;
	private InputAction _mousePosition;
	private InputAction _shootAction;
	private int _hitChain;

	private void Start() {
		_shootAction = InputSystem.actions.FindAction("Attack");
		_mousePosition = InputSystem.actions.FindAction("MousePosition");
		Can.CanGotHit.AddListener(_increaseScore);
		this.StartMinigame();
	}

	private void _increaseScore(int timesHit) {
		Score += timesHit * _hitChain;
	}

	/// <summary>Spawns a can on a random position along the local x-axis with a random angle that keeps the can inside the game space.</summary>
	private void _spawnCan() {
		// TODO: include the randomness
		float posVariation = Random.Range(-_spawnRadius, _spawnRadius);
		float angleVariation = Random.Range(-_spawnRadius, _spawnRadius) - posVariation;
		GameObject canObject = Instantiate(_canPrefab, transform.position + posVariation * transform.forward, Quaternion.identity);
		Can can = canObject.GetComponent<Can>();
		can.LaunchWithForce(Vector3.up * _canLaunchForce + angleVariation * transform.forward);
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

	private IEnumerator _executeEnddelay() {
		_cooldownTimer = _endDelay + 1f;
		_gameRuntime = -1000f;
		yield return new WaitForSeconds(_endDelay);
		_ingame = false;
	}
	
	protected override void _executeGame() {
		_cooldownTimer -= Time.deltaTime;
		_gameRuntime += Time.deltaTime;
		if (_gameRuntime >= _gameDuration) {
			StartCoroutine(_executeEnddelay());
		}
		if (_cooldownTimer < 0) {
			_spawnCan();
			_setCooldown();
		}
		// TODO: Implement click detection here or in Can via OnMouseOver
		if (_shootAction.WasPressedThisFrame()) {
			Vector2 mousePos = _mousePosition.ReadValue<Vector2>();
			Ray aimRay = CameraManager.Instance.MainCamera.ScreenPointToRay(mousePos);
			RaycastHit aimHit;
			if (Physics.Raycast(aimRay, out aimHit)) {
				Can hitCan = aimHit.collider.GetComponent<Can>();
				if (hitCan) {
					_hitChain++;
					Quaternion sparkRotation = Quaternion.FromToRotation(Vector3.forward, -aimRay.direction);
					Destroy(Instantiate(_sparksPrefab, aimHit.point, sparkRotation), 1f);
					hitCan.OnHit();
				} else {
					_hitChain = 0;
				}
			}
		}
	}
	protected override void _finishOffGame() {
		// TODO: this method can be implemented in Minigame instead of being abstract
		_active = false;
		UIManager.Instance.DisplayMinigameEndscreen(this);
	}
}
