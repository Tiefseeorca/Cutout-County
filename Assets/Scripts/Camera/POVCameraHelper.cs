using System;
using Unity.Cinemachine;
using UnityEngine;

/// <summary>
/// Author: Timo Lauterbach
/// </summary>
public class POVCameraHelper : MonoBehaviour {
    private CinemachineInputAxisController _axisController;

    private void Awake() {
        _axisController = GetComponent<CinemachineInputAxisController>();
        // Event listener for enable at game start and disable
        GameManager.GameStarted.AddListener(Enable);
    }

    public void Enable() {
        _axisController.enabled = true;
    }

    public void Disable() {
        _axisController.enabled = false;
    }
}
