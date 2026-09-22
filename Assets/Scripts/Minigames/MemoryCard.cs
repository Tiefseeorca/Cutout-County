using UnityEngine;

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
    }

    public void SetMatched(){
        IsMatched = true;
    }
}
