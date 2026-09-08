using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour{
    public float speed = 3f;
    public float jumpForce = 5f;
    public float groundCheckDistance = 0.3f;
    
    private Rigidbody rb;

    void Start(){
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate(){
        Vector2 input = InputSystem.actions.FindAction("Move").ReadValue<Vector2>();
        Vector3 moveDirection = transform.forward * (-input.x) + transform.right * input.y;
        
        rb.linearVelocity = new Vector3(moveDirection.x * speed, rb.linearVelocity.y, moveDirection.z * speed );
        Debug.Log(input);
    }

    void Update(){
        // Vector3 rayStartPos = transform.position + Vector3.down * 0.8f;
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);
        bool jumpPressed = InputSystem.actions.FindAction("Jump").WasPressedThisFrame();{
            
            if (jumpPressed && isGrounded){
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }
    
    
}