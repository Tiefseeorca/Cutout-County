using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Author: Timo Lauterbach
/// </summary>
[CreateAssetMenu(fileName = "Dialogue", menuName = "Scriptable Objects/Dialogue")]
public class Dialogue : ScriptableObject {
    public static UnityEvent DialogueFinished = new();
    public string Id;
    public List<string> TextBoxes;
    public bool Unique;
    public bool Seen;
    public Flag[] RequiredFlags;
    public Flag[] SetFlags;

    public bool IsAvailable() {
        return !(Unique && Seen) && GameManager.Instance.DoFlagsMatch(RequiredFlags);
    }
}
