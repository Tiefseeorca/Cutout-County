using System;
using Unity.Cinemachine;
using UnityEngine;

public class POVCameraHelper : MonoBehaviour {
    private CinemachineInputAxisController _axisController;
    private bool _enabled;

    private void Awake() {
        _axisController = GetComponent<CinemachineInputAxisController>();
        // Event listener for enable at game start and disable
        //GameManager.GameStarted.AddListener(_enable);
    }

    private void _enable() {
        _enabled = true;
        _axisController.enabled = true;
    }

    private void _disable() {
        _enabled = false;
        _axisController.enabled = false;
    }
}
