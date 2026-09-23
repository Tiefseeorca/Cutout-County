using UnityEngine;
using UnityEngine.InputSystem;

public class Memory : Minigame{
    [SerializeField] private float _gameDuration = 60f;
    [SerializeField] private MemoryCard[] _cards;
    private InputAction _mouseClick;
    private InputAction _mousePosition;
    private float _timeLeft;

    private void Start(){
        _mouseClick = InputSystem.actions.FindAction("Attack");
        _mousePosition = InputSystem.actions.FindAction("MousePosition");
    }
    
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

        if (_mouseClick.WasPressedThisFrame()){
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Ray cardRay = CameraManager.Instance.MainCamera.ScreenPointToRay(mousePos);
            if (Physics.Raycast(cardRay, out RaycastHit hit)){
                MemoryCard card = hit.collider.GetComponent<MemoryCard>();
                if (card != null){
                    Debug.Log("Erfolgreich geklickt auf: " + hit.collider.gameObject.name);
                    card.Flip();
                }
            }
        }
    }

    protected override void _finishOffGame(){
        
    }
    
}
