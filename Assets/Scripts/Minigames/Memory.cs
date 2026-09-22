using UnityEngine;

public class Memory : Minigame{
    [SerializeField] private float _gameDuration = 60f;
    [SerializeField] private MemoryCard[] _cards;
    
    private float _timeLeft;
    

    protected override void _resetGame(){
        _timeLeft = _gameDuration;
        _setupAndShuffleCards();
    }

    private void _setupAndShuffleCards(){
        int[] cardsIDs = new int[16];
        // paare eintragen
        int index = 0;
        for (int i = 0; i < 8; i++){
            cardsIDs[index] = i;
            index++;
            cardsIDs[index] = i;
            index++;
        }
        // Shuffle
        for (int i = 0; i < cardsIDs.Length; i++){
            int temp = cardsIDs[i];
            int randomIndex = Random.Range(i, _cards.Length);
            cardsIDs[i] = cardsIDs[randomIndex];
            cardsIDs[randomIndex] = temp;
        }
        // gemischte IDs zuweisen
        for (int i = 0; i < _cards.Length; i++){
            if (_cards[i] != null){
                _cards[i].SetupCard(cardsIDs[i]);
            }
        }
    }

    protected override void _executeGame(){
        _timeLeft -= Time.deltaTime;
        
        if (_timeLeft <= 0){
            _timeLeft = _gameDuration;
            return;
        }

        if (Input.GetMouseButtonDown(0)){
            Ray cardRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(cardRay, out RaycastHit hit)){
                MemoryCard card = hit.collider.GetComponent<MemoryCard>();
                if (card != null){
                    card.Flip();
                }
            }
        }
    }

    protected override void _finishOffGame(){
        
    }
    
}
