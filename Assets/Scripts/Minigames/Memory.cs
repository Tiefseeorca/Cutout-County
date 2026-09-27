using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Author: Burak Namazci
/// </summary>
public class Memory : Minigame{
    [SerializeField] private float _gameDuration = 40f;
    [SerializeField] private Transform[] _sockets;
    [SerializeField] private GameObject[] _cardPrefabs;
    [SerializeField] private AudioIDs _shuffleSound;
    [SerializeField] private AudioIDs[] _flipSounds;
    
    private bool _introFinished = false;
    private MemoryCard[] _activeCards = new MemoryCard[16];
    private InputAction _mouseClick;
    private InputAction _mousePosition;
    private float _timeLeft;
    private MemoryCard _firstCard;
    private MemoryCard _secondCard;
    private bool _isProcessing = false;
    private int _matchesFound = 0;

    private void Start(){
        _mouseClick = InputSystem.actions.FindAction("Attack");
        _mousePosition = InputSystem.actions.FindAction("MousePosition");
        
        foreach (Transform socket in _sockets){
           // socket.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }
    
    protected override void _resetGame(){
        _timeLeft = _gameDuration;
        _matchesFound = 0;
        _introFinished = false;
        StartCoroutine(_playHandOutCardSound());
    }

    private System.Collections.IEnumerator _playHandOutCardSound(){
        AudioManager.Instance.PlaySfx(_shuffleSound);
        yield return new WaitForSeconds(5f);
        _setupAndShuffleCards();
        _introFinished = true;
    }

    private void _setupAndShuffleCards(){
        int[] cardsIDs = new int[16];
        int index = 0;
        // paare eintragen
        for (int i = 0; i < 8; i++){
            cardsIDs[index] = i;
            index++;
            cardsIDs[index] = i;
            index++;
        }
        // Shuffle
        for (int i = 0; i < cardsIDs.Length; i++){
            int temp = cardsIDs[i];
            int randomIndex = Random.Range(i, cardsIDs.Length);
            cardsIDs[i] = cardsIDs[randomIndex];
            cardsIDs[randomIndex] = temp;
        }
        // gemischte IDs zuweisen
        for (int i = 0; i < _activeCards.Length; i++){
            if (_activeCards[i] != null){ 
                Destroy(_activeCards[i].gameObject);
            }
        }

        for (int i = 0; i < _sockets.Length; i++){
            int prefabID = cardsIDs[i];
            Quaternion cardRotation = _sockets[i].rotation * Quaternion.Euler(90f, 180f, 0f);
            Vector3 spawnPos = _sockets[i].position + new Vector3(0f, 0.01f, 0f);
        
            GameObject spawnedCard = Instantiate(_cardPrefabs[prefabID], spawnPos, cardRotation);
            spawnedCard.transform.SetParent(this.transform);
        
            _activeCards[i] = spawnedCard.GetComponent<MemoryCard>();
            _activeCards[i].SetupCard(prefabID);
        }
    }

    private void _playRandomFlipSound(){
        if (_flipSounds != null && _flipSounds.Length > 0){
            int randomIndex = Random.Range(0, _flipSounds.Length);
            AudioManager.Instance.PlaySfx(_flipSounds[randomIndex]);
        }
    }

    protected override void _executeGame(){
        if (!_introFinished) return;
        _timeLeft -= Time.deltaTime;
        
        if (_timeLeft <= 0){
            _timeLeft = 0;
            _finishOffGame();
            return;
        }

        if (_mouseClick.WasPressedThisFrame()){
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Ray cardRay = CameraManager.Instance.MainCamera.ScreenPointToRay(mousePos);
            
            if (Physics.Raycast(cardRay, out RaycastHit hit)){
                MemoryCard card = hit.collider.GetComponent<MemoryCard>();

                if (card != null && !card.IsMatched && !card.IsFlipped && !_isProcessing){
                    if (_firstCard == null){
                        _firstCard = card;
                        _firstCard.Flip();
                        _playRandomFlipSound();
                    } else if (_secondCard == null && card != _firstCard){
                        _secondCard = card;
                        _secondCard.Flip();
                        _playRandomFlipSound();
                        
                        _isProcessing = true;
                        StartCoroutine(_checkMatch());
                    }
                }
            }
        }
    }

    private System.Collections.IEnumerator _checkMatch(){
        if (_firstCard.CardID == _secondCard.CardID){
            _firstCard.SetMatched();
            _secondCard.SetMatched();
            _matchesFound++;
            Score += 50;
            
        } else {
            Score -= 20;
            if (Score < 0) Score = 0;
            
            yield return new WaitForSeconds(1f);
            _firstCard.Flip();
            _secondCard.Flip();
        }
        if (Score > Highscore) Highscore = Score;
        UIManager.Instance.UpdateMinigameScore(Score);
        
        if (_matchesFound == 8){
            // Debug.Log("Alle Paare gefunden.");
            _finishOffGame();
        }
        _firstCard = null;
        _secondCard = null;
        _isProcessing = false;
    }
    
    protected override void _finishOffGame(){
        _active = false;
        _ingame = false;

        if (_matchesFound == 8){ 
            int timeBonus = Mathf.FloorToInt(_timeLeft * 10f);
            Score += timeBonus; 
        }
        
        if (Score > Highscore){
            Highscore = Score;
        }
        
        UIManager.Instance.UpdateMinigameScore(Score);
        UIManager.Instance.DisplayMinigameEndscreen(this);
    }
}
