
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelControlInverter : MonoBehaviour
{
   private Rigidbody2D rb;
   private Player playerScript;
   private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerScript = GetComponent<Player>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if(playerScript == null || rb == null) return;

        float moveDir = 0f;

        if(Keyboard.current != null)
        {
            if(Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            {
                moveDir = 1f;
            }
            else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            {
               moveDir = -1f;
            }
        }
        rb.linearVelocity = new Vector2(moveDir * playerScript.speed, rb.linearVelocity.y);

        if(spriteRenderer != null)
        {
            if(moveDir > 0f)
            {
                spriteRenderer.flipX = false;
            }
            else if(moveDir < 0f)
            {
                spriteRenderer.flipX = true;

            }
        }
    }
}
