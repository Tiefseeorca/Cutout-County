using System;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour {
	public static CameraManager Instance;
	[SerializeField] private CinemachineCamera[] Cameras;

	[SerializeField] private CinemachineCamera StartCamera;
	public CinemachineCamera PlayerCamera;
	private CinemachineCamera _currentCamera;

	[SerializeField] private int ActivePriority;
	[SerializeField] private int InactivePriority;

	private void Awake() {
		Instance = this;
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
