using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

/// <summary>
/// Author: Timo Lauterbach
/// </summary>
public class CanShootingRange : Minigame {
	[SerializeField] private GameObject _canPrefab;
	[SerializeField] private GameObject _sparksPrefab;
	[SerializeField] private float _startCanCooldown;
	[SerializeField] private float _minCanCooldown;
	[SerializeField] private float _gameDuration;
	[SerializeField] private float _canLaunchForce;
	[SerializeField] private float _spawnRadius;
	[SerializeField] private Texture2D _cursorTexture;
	[SerializeField] private Vector2 _cursorHotspot;
	private float _gameRuntime;
	private float _cooldownTimer;
	private InputAction _mousePosition;
	private InputAction _shootAction;
	private int _hitChain;
	[SerializeField] private AudioIDs _shootingSound;
	[SerializeField] private AudioIDs _canHitSound;
	private const float _chainPitchIncrease = 0.02f;
	private const float _comboPitchIncrease = 0.4f;

	private void Start() {
		_shootAction = InputSystem.actions.FindAction("Attack");
		_mousePosition = InputSystem.actions.FindAction("MousePosition");
		Can.CanGotHit.AddListener(_increaseScore);
	}

	/// <summary>
	/// This function gets called upon a can getting hit and increases the score based on hit count on that particular can,
	///	as well as how many cans have been hit without missing.
	/// </summary>
	/// <param name="timesHit">The amount of time the hit can has already been hit.</param>
	private void _increaseScore(int timesHit) {
		float pitch = 1 + _hitChain * _chainPitchIncrease + (timesHit - 1) * _comboPitchIncrease;
		AudioManager.Instance.PlaySfx(_canHitSound, transform, 1, pitch);
		Score += timesHit * _hitChain;
		if (Score > Highscore) Highscore = Score;
		UIManager.Instance.UpdateMinigameScore(Score);
	}

	/// <summary>Spawns a can on a random position along the local x-axis with a random angle that keeps the can inside the game space.</summary>
	private void _spawnCan() {
		// TODO: include the randomness
		float posVariation = Random.Range(-_spawnRadius, _spawnRadius);
		float angleVariation = Random.Range(-_spawnRadius, _spawnRadius) - posVariation;
		GameObject canObject = Instantiate(_canPrefab, transform.position + posVariation * transform.right, Quaternion.identity);
		Can can = canObject.GetComponent<Can>();
		can.LaunchWithForce(Vector3.up * _canLaunchForce + angleVariation * transform.right);
	}

	/// <summary>Sets the cooldown for the next can to spawn. Speeds up linearly with game progression.</summary>
	private void _setCooldown() {
		float rel = _gameRuntime / _gameDuration;
		_cooldownTimer = (1 - rel) * _startCanCooldown + rel * _minCanCooldown;
	}

	protected override void _resetGame() {
		_gameRuntime = 0;
		_cooldownTimer = _startDelay;
		_hitChain = 0;
		Vector2 newCursorHotspot = new Vector2(_cursorTexture.width * _cursorHotspot.x, _cursorTexture.height * _cursorHotspot.y);
		Cursor.SetCursor(_cursorTexture, newCursorHotspot, CursorMode.Auto);
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
		if (_shootAction.WasPressedThisFrame()) {
			_shoot();
		}
	}

	private void _shoot() {
		AudioManager.Instance.PlaySfx(_shootingSound);
		Vector2 mousePos = _mousePosition.ReadValue<Vector2>();
		Ray aimRay = CameraManager.Instance.MainCamera.ScreenPointToRay(mousePos);
		RaycastHit aimHit;
		if (Physics.Raycast(aimRay, out aimHit, Mathf.Infinity, LayerMask.GetMask("Can"))) {
			Can hitCan = aimHit.collider.GetComponent<Can>();
			if (hitCan) {
				_hitChain++;
				Quaternion sparkRotation = Quaternion.FromToRotation(Vector3.forward, -aimRay.direction);
				Destroy(Instantiate(_sparksPrefab, aimHit.point, sparkRotation), 1f);
				hitCan.OnHit();
			} 
		} else {
			_hitChain = 0;
		}
	}
	protected override void _finishOffGame() {
		// TODO: this method can be implemented in Minigame instead of being abstract
		_active = false;
		UIManager.Instance.DisplayMinigameEndscreen(this);
	}
}
