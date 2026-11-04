using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.Rendering.UI;

public class rocketController : MonoBehaviour
{
    public float thrustPower = 12f;
    public float steeringSpeed = 8f;
    // public float upwardGravity = 1.5f;

   public float maxtiltAngle = 25f;
   public float tiltsmoothSpeed = 10f;

   public Transform spriteTransform;


    public CinemachineCamera cinemachineCam;
    public Transform playertransform;


    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private bool isPiloting = false;
    private bool isOutOfFuel = false;

    private int coinCount = 0;

    private float currentTilt = 0f;

    public float maxFuel = 100f;
    public float fuelBurnRate = 12f;
    private float currentFuel;

    public TextMeshProUGUI coinText;
    public TextMeshProUGUI fuelText;

    public float maxLandingSpeed = 4f;
    public string nextLevelName = "Level2";





    void Start()
    {
        rb = GetComponent<Rigidbody2D>() ;
        boxCollider = GetComponent<BoxCollider2D>();
        if(spriteTransform == null) 
        spriteTransform = transform;

        currentFuel = maxFuel;
        UpdateUI();




    }

    public void StartPiloting()
    {
        isPiloting = true;

        if(boxCollider != null)
        {
            boxCollider.isTrigger = false;
            
        }
        
        if(rb != null
        )
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 0.5f;
            rb.linearDamping = 1.5f;

        }

        if(cinemachineCam != null)
        {
            cinemachineCam.Follow = transform;

        }
    }

    void FixedUpdate()
    {
        if(!isPiloting) 
        return;

        float moveHorizontal = 0f;
        float moveVertical = 0f;

        if(!isOutOfFuel && Keyboard.current != null)
        {
            if(Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) moveHorizontal = -1f;
            if(Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) moveHorizontal = 1f;
            if(Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) moveVertical = 1f;
            if(Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) moveVertical = -1f;

        }

        if(!isOutOfFuel && (moveHorizontal != 0f || moveVertical != 0f))
        {
            currentFuel -= fuelBurnRate * Time.fixedDeltaTime;

            if(currentFuel <= 0f)
            {
                currentFuel = 0f;
                isOutOfFuel = true;
                rb.gravityScale = 2f;

            }
            Vector2 force = new Vector2(moveHorizontal * steeringSpeed, moveVertical * thrustPower);
        rb.AddForce(force);

        }
        UpdateUI();

        float targetTilt = isOutOfFuel ? 0f : (-moveHorizontal * maxtiltAngle);

        currentTilt = Mathf.Lerp(currentTilt, targetTilt, tiltsmoothSpeed * Time.fixedDeltaTime);

        if(spriteTransform != null)
        {
            spriteTransform.rotation = Quaternion.Euler(0f, 0f, currentTilt);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        Debug.Log("Hit Object: " + collision.gameObject.name + "with tag: " + collision.tag);
        if(collision.CompareTag("Coin"))
        {
            if(GameManager.Instance != null)
            {
                GameManager.Instance.AddCoins(1);
            }

            Destroy(collision.gameObject);
            UpdateUI();
        }

        else  if(collision.CompareTag("Fuel"))
        {
            currentFuel = maxFuel; 
            isOutOfFuel = false;
            rb.gravityScale = 0.5f; 
            Destroy(collision.gameObject);
            UpdateUI();
        }


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       if(!isPiloting) return;
       if(collision.gameObject.CompareTag("LandingPad"))
        {
            float landingSpeed = rb.linearVelocity.magnitude;

            if(landingSpeed <= maxLandingSpeed)
            {
                LoadNextLevel();
            }
             else
            {
                RestartGame();

            }
        }

        else if(collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Ground"))
        {
            RestartGame();
        }
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(nextLevelName);
    }


    private void UpdateUI()
    {
        if(coinText != null)
        {
            int displayCoins = (GameManager.Instance != null) ? GameManager.Instance.totalCoins : 0;
            coinText.text = "Total Coins :  " + displayCoins;

        }

        if(fuelText != null)
            fuelText.text = "Fuel : " + Mathf.Round(currentFuel) + "%";
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
