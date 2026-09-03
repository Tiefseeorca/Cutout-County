using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour{
    public float speed = 3f;
    public float jumpForce = 5f;
    
    private Rigidbody rb;

    void Start(){
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate(){
        Vector2 input = InputSystem.actions.FindAction("Move").ReadValue<Vector2>();
        Vector3 moveDirection = transform.forward * input.y + transform.right * input.x;
        
        rb.linearVelocity = new Vector3(moveDirection.x * speed, rb.linearVelocity.y, moveDirection.z * speed );
        Debug.Log(input);
    }

    void Update(){
        bool jumpPressed = InputSystem.actions.FindAction("Jump").WasPressedThisFrame();
        if (jumpPressed){
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    
    
}