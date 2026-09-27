using System;
using Unity.Cinemachine;
using UnityEngine;

/// <summary>
/// Author: Timo Lauterbach
/// </summary>
public class CameraManager : MonoBehaviour {
	public static CameraManager Instance;
	[SerializeField] private CinemachineCamera[] Cameras;

	[SerializeField] private CinemachineCamera StartCamera;
	public CinemachineCamera PlayerCamera;
	private CinemachineCamera _currentCamera;
	public Camera MainCamera;

	[SerializeField] private int ActivePriority;
	[SerializeField] private int InactivePriority;

	private void Awake() {
		Instance = this;
		MainCamera = Camera.main;
		_currentCamera = StartCamera;
		foreach (CinemachineCamera camera in Cameras) {
			if (camera == _currentCamera) {
				_currentCamera.Priority = ActivePriority;
			} else {
				camera.Priority = InactivePriority;
			}
		}
	}

	public void SwitchTo(CinemachineCamera camera) {
		_currentCamera.Priority = InactivePriority;
		_currentCamera = camera;
		_currentCamera.Priority = ActivePriority;
	}
	
	
}
