using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class rocketController : MonoBehaviour
{
    public float thrustPower = 12f;
    public float steeringSpeed = 8f;
    // public float upwardGravity = 1.5f;

    public CinemachineCamera cinemachineCam;
    public Transform playertransform;


    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private bool isPiloting = false;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>() ;
        boxCollider = GetComponent<BoxCollider2D>();



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

        if(Keyboard.current != null)
        {
            if(Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) moveHorizontal = -1f;
            if(Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) moveHorizontal = 1f;
            if(Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) moveVertical = 1f;
            if(Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) moveVertical = -1f;

        }

        Vector2 force = new Vector2(moveHorizontal * steeringSpeed, moveVertical * thrustPower);
        rb.AddForce(force);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(isPiloting && (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Ground")))
        {
            RestartGame() ;
        }


    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
