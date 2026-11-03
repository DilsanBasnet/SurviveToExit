using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    public float speed = 8f;
    public float jump = 12f;
    public Transform groundCheck;

    public Vector2 groundCheckSize = new Vector2(0.5f, 0.1f);
    public LayerMask groundLayer;


    public Transform wallCheckRight;
    public Transform wallCheckleft;
    public Vector2 wallCheckSize = new Vector2(0.2f, 0.5f);
    public LayerMask wallLayer;
    public float wallSlidingSpeed = 2f;
    public Vector2 wallJumpingPower = new Vector2(8f, 12f);



    private bool isWallSliding;
    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private float wallJumpingTime = 0.2f;

    


    public Animator anim;

    private Rigidbody2D rb;
    private float move;

    private SpriteRenderer spriteRenderer;

    private bool Grounded;

    private bool canDoubleJump;

    private bool isTouchingRightWall;
    private bool isTouchingLeftWall;



    void Start()
    {
        rb = GetComponent<Rigidbody2D> ();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if(anim == null)
        {
            anim = GetComponent<Animator>();
        }

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

        if(anim != null)
        {
            bool isMoving = (move != 0f);
            anim.SetBool("isRunning", isMoving);
        }

        if(spriteRenderer != null && !isWallJumping)
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

        if(Grounded)
        {
            canDoubleJump = true;
            if(anim != null) anim.SetBool("isJumping", false);
                  }

                  CheckWalls();
                  WallSlide();
                  WallJumpLogic();


        bool jumpPressed = Keyboard.current != null && (Keyboard.current.wKey.wasPressedThisFrame || Keyboard.current.upArrowKey.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame);

        if(jumpPressed)
        {

            if(isWallJumping)
            {
                rb.linearVelocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
                wallJumpingCounter = 0f;

                if(spriteRenderer != null) 
                spriteRenderer.flipX = wallJumpingDirection > 0 ? false : true;

                isWallJumping = false;

            }
           else  if(Grounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jump);
                if(anim != null) anim.SetBool("isJumping", true) ;
                
                        }

            else if(canDoubleJump)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jump) ;
                canDoubleJump = false;
                if(anim != null) anim.SetBool("isJumping", true);
            }
        }


      
    }

    private void CheckWalls()
    {
        isTouchingRightWall = false;
        isTouchingLeftWall = false;

        if(wallCheckRight != null)
        {
            isTouchingRightWall= Physics2D.OverlapBox(wallCheckRight.position, wallCheckSize, 0f, wallLayer);
        }

        if(wallCheckleft != null)
        {
            isTouchingLeftWall = Physics2D.OverlapBox(wallCheckleft.position, wallCheckSize, 0f, wallLayer);

        }
        if(!Grounded && (isTouchingRightWall || isTouchingLeftWall))
        {
            wallJumpingDirection = isTouchingRightWall ? -1f : 1f;
            wallJumpingCounter = wallJumpingTime;
            CancelInvoke(nameof(StopWallJumping));

        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;

        }
    }

    private void WallSlide()
    {
        if(!Grounded && (isTouchingRightWall || isTouchingLeftWall) && move != 0f)
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -wallSlidingSpeed));

        }
        else
        {
            isWallSliding = false;

        }
    }

    
    private void WallJumpLogic()
    {
        bool jumpPressed = Keyboard.current != null && (Keyboard.current.wKey.wasPressedThisFrame || Keyboard.current.upArrowKey.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame);

        if(wallJumpingCounter > 0 && jumpPressed)
        {
            isWallJumping = true;
            wallJumpingDirection = spriteRenderer.flipX ? 1f : -1f;

            rb.linearVelocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);

            wallJumpingCounter = 0f;

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }
    

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Ground"))
        {
            if(anim != null)  anim.SetBool("isJumping", false);

        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Ground"))
        {
           if(anim != null) anim.SetBool("isJumping", true);

        }
    }

    
    void FixedUpdate()
    {
        if(!isWallJumping)
        {
            rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y) ;
        }  
    }
}
