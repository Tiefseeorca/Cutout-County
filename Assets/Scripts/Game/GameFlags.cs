using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public struct Flag {
    public string Id;
    public bool Value;

    public Flag(string id, bool value) {
        Id = id;
        Value = value;
    }

    public void SetValue(bool val) {
        Value = val;
    }
}

[CreateAssetMenu(fileName = "GameFlags", menuName = "Scriptable Objects/GameFlags")]
public class GameFlags : ScriptableObject {
    public static UnityEvent<Flag> FlagChanged = new();
    [SerializeField] private Flag[] _gameFlags;

    private void Awake() {
        for (int i = 0; i < _gameFlags.Length; i++) {
            _gameFlags[i] = new Flag(_gameFlags[i].Id, _gameFlags[i].Value);
        }
    }

    public void SetGameFlag(string id, bool val) {
        for (int i = 0; i < _gameFlags.Length; i++) {
            if (_gameFlags[i].Id == id) {
                _gameFlags[i].SetValue(val);
                FlagChanged.Invoke(_gameFlags[i]);
                return;
            }
        }
    }
}
