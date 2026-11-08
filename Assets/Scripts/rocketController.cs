using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

public class rocketController : MonoBehaviour
{
    public float thrustPower = 12f;
    public float steeringSpeed = 8f;

   public float maxtiltAngle = 25f;
   public float tiltsmoothSpeed = 10f;

   public Transform spriteTransform;


    public CinemachineCamera cinemachineCam;
    public Transform playertransform;

    public bool useFixedCameraForPlayer = true;

    public GameObject playerCameraObject;
    public GameObject rocketCameraObject;

    public ParticleSystem explosionParticles;

    public AudioClip coinCollectSfx;
    public AudioClip fuelCollectSfx;
    public AudioClip crashSfx;
    public AudioClip successfullLandingSfx;
    public AudioClip rocketFlightLoopSfx;
    public GameObject floatingTextPrefab;



    private bool isCrashing = false;
private bool hasLanded = false;


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

    private AudioSource audioSource;





    void Start()
    {
        rb = GetComponent<Rigidbody2D>() ;
        boxCollider = GetComponent<BoxCollider2D>();
        if(spriteTransform == null) 
        spriteTransform = transform;

        audioSource = GetComponent<AudioSource>();

        if(audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        currentFuel = maxFuel;
        UpdateUI();

        if(playerCameraObject != null ) playerCameraObject.SetActive(true);
        if(rocketCameraObject != null) rocketCameraObject.SetActive(false);

        if(cinemachineCam != null && playerCameraObject == null) 
        {
            if(useFixedCameraForPlayer)
            {
                cinemachineCam.gameObject.SetActive(false);
            }
            else
            {
                cinemachineCam.gameObject.SetActive(true);

                if(playertransform != null)
                {
                    cinemachineCam.Follow = playertransform;
                }
            }
        }
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

        if(rocketFlightLoopSfx != null && audioSource != null)
        {
            audioSource.clip = rocketFlightLoopSfx;
            audioSource.loop = true;
            audioSource.Play();
        }

        if(playerCameraObject != null && rocketCameraObject != null)
        {
            playerCameraObject.SetActive(false);
            rocketCameraObject.SetActive(true);

        }
        else if(cinemachineCam != null)
        {
            cinemachineCam.gameObject.SetActive(true);
            cinemachineCam.Follow = transform;
        }
    }

    void FixedUpdate()
    {
        if(!isPiloting || hasLanded) 
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

        if(collision.CompareTag("Coin"))
        {
            if(GameManager.Instance != null)
            {
                GameManager.Instance.AddRocketCoins(1);
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
                tmp.text = "+1 COINS";
            }
        }
              
            }


PlaySound(coinCollectSfx);
            Destroy(collision.gameObject);
            UpdateUI();
        }

        else  if(collision.CompareTag("Fuel"))
        {
            currentFuel = maxFuel; 
            isOutOfFuel = false;
            rb.gravityScale = 0.5f; 
            PlaySound(fuelCollectSfx);
            Destroy(collision.gameObject);
            UpdateUI();
        }

        else if(isPiloting && (collision.CompareTag("DeathZone") || collision.CompareTag("Obstacle")))
        {
            CrashAndRestart();
        }


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       if(!isPiloting || hasLanded) return;
       if(collision.gameObject.CompareTag("LandingPad"))
        {
            float landingSpeed = rb.linearVelocity.magnitude;

            if(landingSpeed <= maxLandingSpeed)
            {
                HandleSuccessfulLanding();
            }
             else
            {
                CrashAndRestart();

            }
        }

        else if(collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Spikes"))
        {
            CrashAndRestart();
        }
    }

    private void LoadNextLevel()
    {
        if(SceneFader.Instance != null)
        {
            SceneFader.Instance.FadeToLevel(nextLevelName);
        }

        else
        {
            SceneManager.LoadScene(nextLevelName);
        }
    }


    private void UpdateUI()
    {
        if(coinText != null)
        {
            int displayCoins = (GameManager.Instance != null) ? GameManager.Instance.rocketCoins : 0;
            coinText.text = "COINS :" + displayCoins;

        }

        if(fuelText != null)
            fuelText.text = "Fuel : " + Mathf.Round(currentFuel) + "%";
    }

    private void RestartGame()
    {
       string currentSceneName = SceneManager.GetActiveScene().name;

       if(SceneFader.Instance != null)
        {
            SceneFader.Instance.FadeToLevel(currentSceneName);
        }
        else
        {
            SceneManager.LoadScene(currentSceneName);
        }


    }

    private void CrashAndRestart()
    {
       if(isCrashing) return;
       isCrashing = true;
       isPiloting = false;

       if(audioSource != null)
        {
            audioSource.Stop();
        }

       if(rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
        }

        if(spriteTransform != null)
        {
            SpriteRenderer sr = spriteTransform.GetComponent<SpriteRenderer>();
            if(sr != null)  sr.enabled = false;

        }
        PlaySound(crashSfx);

        if(explosionParticles != null)
        {
            explosionParticles.transform.position = transform.position;
            explosionParticles.gameObject.SetActive(true);
            explosionParticles.Play();

            float duration = explosionParticles.main.duration;
            StartCoroutine(RestartAfterDelay(duration));
        }
        else
        {
            RestartGame();
        }
    }

    private System.Collections.IEnumerator RestartAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        RestartGame();
    }

    private void HandleSuccessfulLanding()
    {
        hasLanded = true;

        if(audioSource != null)
        {
            audioSource.Stop();
        }
        if(rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;

        }

        PlaySound(successfullLandingSfx);

        float delay = successfullLandingSfx != null ? successfullLandingSfx.length : 1.5f;
        StartCoroutine(LoadNextLevelAfterDelay(delay));
    }

    private System.Collections.IEnumerator LoadNextLevelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay) ;
        SceneManager.LoadScene(nextLevelName);
    }

    private void PlaySound(AudioClip clip)
    {
        if(clip != null && audioSource != null)
        {
           audioSource.PlayOneShot(clip);
        }
    }

    public void TriggerExternalCrash()
    {
       
        CrashAndRestart();
    }


}
