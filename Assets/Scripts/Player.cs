using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    public float speed = 8f;
    public float jump = 12f;
    public Transform groundCheck;

    public Vector2 groundCheckSize = new Vector2(0.5f, 0.1f);
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private float move;

    private SpriteRenderer spriteRenderer;

    private bool Grounded;


    void Start()
    {
        rb = GetComponent<Rigidbody2D> ();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

    }

    void Update()
    {
        move = 0f;

    if(Keyboard.current != null)
        {
            if(Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) 
            move = -1f;

            else if(Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            move = 1f;
        }

        if(spriteRenderer != null)
        {
            if(move > 0) 
            spriteRenderer.flipX = false;

            else if(move < 0) 
            spriteRenderer.flipX = true;
        }

        if(groundCheck != null)
        {
            Grounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer);
        }

        bool jumpPressed = Keyboard.current != null && (Keyboard.current.wKey.wasPressedThisFrame || Keyboard.current.upArrowKey.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame);

        if(jumpPressed && Grounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jump);
        }
    }
    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y) ;
        
    }
}
