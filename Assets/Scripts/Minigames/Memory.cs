using UnityEngine;

public class Memory : Minigame{
    [SerializeField] private float _gameDuration;
    private float _timeLeft;

    protected override void _resetGame(){
        _timeLeft = _gameDuration;
    }

    protected override void _executeGame(){
        _timeLeft -= Time.deltaTime;
        
        if (_timeLeft <= 0) 
            _ingame = false;
        return;
    }

    protected override void _finishOffGame(){
        
    }
    
}
