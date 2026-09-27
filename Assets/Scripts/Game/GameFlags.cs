using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Author: Timo Lauterbach
/// </summary>
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

/// <summary>
/// <p>A GameFlags object holds information about the state of the world, which determines which events can happen,
///     what dialogue is available and which quests get activated or completed.</p>
/// <p>Author: Timo Lauterbach</p>
/// </summary>
[CreateAssetMenu(fileName = "GameFlags", menuName = "Scriptable Objects/GameFlags")]
public class GameFlags : ScriptableObject {
    public static UnityEvent<Flag> FlagChanged = new();
    [SerializeField] private Flag[] _gameFlags;

    public Flag GetFlagById(string Id) {
        foreach (Flag flag in _gameFlags) {
            if (flag.Id == Id) return flag;
        }
        return new Flag("Empty", false);
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
