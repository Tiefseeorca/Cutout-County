using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Scriptable Objects/Dialogue")]
public class Dialogue : ScriptableObject {
    public string Text;
    public string Id;
    public bool Unique;
    public bool Seen;
    public Flag[] RequiredFlags;
    public Flag[] SetFlags;

    public bool IsAvailable() {
        throw new NotImplementedException("TODO");
    }
}
