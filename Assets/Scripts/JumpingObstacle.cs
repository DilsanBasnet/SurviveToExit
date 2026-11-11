using System.Collections;
using System.Data;
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpingObstacle : MonoBehaviour
{
   [SerializeField] private float jumpForce = 20f;
   [SerializeField] private float resetDelay = 0.5f;

   private Rigidbody2D rb;
   private Vector3 startPosition;
   private bool isJumping = false;


   void Start()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();

        if(rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.freezeRotation = true;
        }
    }

    void Update()
    {
        if(Keyboard.current != null)
        {
            bool jumpPressed = Keyboard.current.wKey.wasPressedThisFrame || 
                               Keyboard.current.upArrowKey.wasPressedThisFrame || 
                               Keyboard.current.spaceKey.wasPressedThisFrame;

        

        if(jumpPressed && !isJumping)
            {
                StartCoroutine(TriggerSpikeJump());
            }
        }
    }

    private IEnumerator TriggerSpikeJump()
    {
        isJumping = true;
        rb.bodyType = RigidbodyType2D.Dynamic;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        yield return new WaitForSeconds(resetDelay);

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;
        transform.position = startPosition;

        isJumping = false;
        
    }
}
