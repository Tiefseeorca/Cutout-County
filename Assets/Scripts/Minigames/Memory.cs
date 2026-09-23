using UnityEngine;
using UnityEngine.InputSystem;

public class Memory : Minigame{
    [SerializeField] private float _gameDuration = 60f;
    [SerializeField] private Transform[] _sockets;
    [SerializeField] private GameObject[] _cardPrefabs;
    
    private MemoryCard[] _activeCards = new MemoryCard[16];
    private InputAction _mouseClick;
    private InputAction _mousePosition;
    private float _timeLeft;

    private void Start(){
        _mouseClick = InputSystem.actions.FindAction("Attack");
        _mousePosition = InputSystem.actions.FindAction("MousePosition");

        foreach (Transform socket in _sockets){
           // socket.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }
    
    protected override void _resetGame(){
        _timeLeft = _gameDuration;
        _setupAndShuffleCards();
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
            Quaternion flatRotation = _sockets[i].rotation * Quaternion.Euler(90f, 180f, 0f);
        
            GameObject spawnedCard = Instantiate(_cardPrefabs[prefabID], _sockets[i].position, flatRotation);
            spawnedCard.transform.SetParent(this.transform); 
        
            _activeCards[i] = spawnedCard.GetComponent<MemoryCard>();
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
