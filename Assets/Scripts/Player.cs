using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.Cinemachine;


public class Player : MonoBehaviour
{

    public float speed = 8f;
    public float jump = 12f;
    public Transform groundCheck;

    public Vector2 groundCheckSize = new Vector2(0.2f, 0.1f);
    public LayerMask groundLayer;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2.0f;



    public Transform wallCheckRight;
    public Transform wallCheckleft;
    public Vector2 wallCheckSize = new Vector2(0.2f, 0.5f);
    public LayerMask wallLayer;
    public float wallSlidingSpeed = 2f;
    public Vector2 wallJumpingPower = new Vector2(8f, 12f);


    public bool enableDoubleJump = true;
    public bool enableWallJump = true;
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

    public TextMeshProUGUI playerCoinText;

    public AudioClip jumpSfx;
    public AudioClip coinCollectSfx;
    public AudioClip deathSfx;

    public GameObject deathParticlePrefab;
    public GameObject floatingTextPrefab;

    private bool isDead = false;
    private AudioSource audioSource;

    public CinemachineCamera playerCinemachineCam;
    private CinemachineBasicMultiChannelPerlin playerPerlin;
    private float shakeTimer;




    void Start()
    {
        rb = GetComponent<Rigidbody2D> ();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        audioSource = GetComponent<AudioSource>();
        if(audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
                   
                    }
        if(anim == null)
        {
            anim = GetComponent<Animator>();
        }
        UpdatePlayerUI();


        if(playerCinemachineCam != null)
        {
            playerPerlin = playerCinemachineCam.GetComponent<CinemachineBasicMultiChannelPerlin>();
        }

    }

    void Update()
    {
        if(shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if(shakeTimer <= 0 && playerPerlin != null)
            {
                playerPerlin.AmplitudeGain = 0f;
                playerPerlin.FrequencyGain = 0f;
            }
        }

        if(isDead) return;

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
            Grounded = Physics2D.Raycast(groundCheck.position, Vector2.down, 0.14f, groundLayer);
        }

        if(Grounded)
        {
            canDoubleJump = true;
            if(anim != null) anim.SetBool("isJumping", false);
                  }

if(enableWallJump)
        {
            CheckWalls();
                  WallSlide();
                  WallJumpLogic();

        }
        else
        {
            isWallJumping = false;
            isWallSliding = false;
            wallJumpingCounter = 0f;

        }
                  

        bool jumpPressed = Keyboard.current != null && (Keyboard.current.wKey.wasPressedThisFrame || Keyboard.current.upArrowKey.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame);

        if(jumpPressed)
        {

            if(enableWallJump && wallJumpingCounter > 0)
            {
                rb.linearVelocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
                wallJumpingCounter = 0f;

                if(spriteRenderer != null) 
                spriteRenderer.flipX = wallJumpingDirection > 0 ? false : true;

                isWallJumping = false;
                PlaySound(jumpSfx);

            }
           else  if(Grounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jump);
                if(anim != null) anim.SetBool("isJumping", true) ;
                PlaySound(jumpSfx);
                
                        }

            else if(enableDoubleJump && canDoubleJump)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jump) ;
                canDoubleJump = false;
                if(anim != null) anim.SetBool("isJumping", true);
                PlaySound(jumpSfx);
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
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Ground"))
        {
            if(anim != null)  anim.SetBool("isJumping", false);

        }
        if(other.gameObject.CompareTag("Obstacle") || other.gameObject.GetComponent<Obstacle>() != null)
        {
            TriggerDeath();
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Ground"))
        {
           if(anim != null) anim.SetBool("isJumping", true);

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerCoin"))
        {
            if(GameManager.Instance != null)
            {
                GameManager.Instance.AddPlayerCoins(1);
            }

            if(floatingTextPrefab != null)
            {
               Canvas canvas = FindAnyObjectByType<Canvas>();
        if(canvas != null)
        {
            Vector3 screenPoint = Camera.main.WorldToScreenPoint(collision.transform.position + new Vector3(0f, 0.5f, 0f));
            GameObject textObj = Instantiate(floatingTextPrefab, screenPoint, Quaternion.identity, canvas.transform);
            
            TextMeshProUGUI tmp = textObj.GetComponent<TextMeshProUGUI>();
            if(tmp != null)
            {
                tmp.text = "+1 GEMS";
            }
        }
            }
            PlaySound(coinCollectSfx);
            Destroy(collision.gameObject);
            UpdatePlayerUI();
        }
        else if(collision.CompareTag("Rocket"))
        {
         rocketController rocket = collision.GetComponent<rocketController>();
         
         if(rocket!= null)
            {
                rocket.StartPiloting();
                gameObject.SetActive(false);
                
            }
        }
        if(collision.CompareTag("DeathZone") || collision.CompareTag("Obstacle") || collision.GetComponent<Obstacle>() != null)
        {
           TriggerDeath();

        }
    }

    private void TriggerDeath()
    {
        if(isDead) return;
        isDead = true;

        TriggerPlayerCameraShake(3f, 0.3f);

        MonoBehaviour fallRespawnScript = GetComponent("FallRespawn") as MonoBehaviour;

        if(fallRespawnScript != null)
        {
            fallRespawnScript.enabled = false;

        }

        PlaySound(deathSfx);
        if(deathParticlePrefab != null)
        {
            Instantiate(deathParticlePrefab, transform.position, Quaternion.identity);
        }

        if(spriteRenderer != null) spriteRenderer.enabled = false;
        if(rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;

        }

        Collider2D col = GetComponent<Collider2D>();
        if(col != null) col.enabled = false;

        Invoke(nameof(RestartGame), 0.8f);
    }

public void TriggerPlayerCameraShake(float intensity, float duration)
    {
        if(playerPerlin != null)
        {
            playerPerlin.AmplitudeGain = intensity;
            playerPerlin.FrequencyGain = intensity;
            shakeTimer = duration;
        }
    }
    public void TriggerExternalDeath()
    {
        TriggerDeath();
    }

    private void PlaySound(AudioClip clip)
    {
        if(clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private void UpdatePlayerUI()
    {
        if(playerCoinText != null)
        {
            int displayPlayerCoins = (GameManager.Instance != null)  ? GameManager.Instance.PlayerCoins : 0;
            playerCoinText.text = "GEMS : " + displayPlayerCoins;

        }
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    
    void FixedUpdate(){

        if(isDead) return;
        
        if(!isWallJumping)
        {
            rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y) ;
        }

        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1f) * Time.fixedDeltaTime;
            
      }
      else if (rb.linearVelocity.y > 0 && Keyboard.current != null && !(Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed || Keyboard.current.spaceKey.isPressed))
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1f) * Time.fixedDeltaTime;
        }
    }
}
