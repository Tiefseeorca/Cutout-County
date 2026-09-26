using UnityEngine;

/// <summary>
/// Author: Burak Namazci
/// </summary>
public class MemoryCard : MonoBehaviour{
    public int CardID {get; private set;}
    public bool IsFlipped {get; private set;}
    public bool IsMatched {get; private set;}
    
    public void SetupCard(int newID){
        CardID = newID;
        IsFlipped = false;
        IsMatched = false;
    }

    public void Flip(){
        IsFlipped = !IsFlipped;
        transform.Rotate(0f, 180f, 0f);
        }

    public void SetMatched(){
        IsMatched = true;
    }
}