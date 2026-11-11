using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class OppositePlayer : MonoBehaviour
{
   private bool isRunning;
   private bool hasSpawned = false;

   [SerializeField] private Animator animator;

   [SerializeField] private Key right = Key.D;
   [SerializeField] private Key left = Key.A;
   [SerializeField] private float moveSpeed = 4f;
   [SerializeField] private float jumpForce = 7f;
   private AudioSource sourceRestart;
   [SerializeField] private AudioClip audioRestart;
   [SerializeField, Min(0.2f)] private float loadDelay = 0.5f;

   private Rigidbody2D rb;
   private SpriteRenderer _spriteRender;


public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Awake()
    {
        sourceRestart = GetComponent<AudioSource>();

        if(sourceRestart == null)
        {
            sourceRestart = gameObject.AddComponent<AudioSource>();
        }

        rb = GetComponent<Rigidbody2D>();
        _spriteRender = GetComponent<SpriteRenderer>();

        if(_spriteRender != null)
        {
            _spriteRender.enabled = true;

        }
            }


         [Obsolete] 
         private void Update()
    {
        if(Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
        {
            if(sourceRestart != null && audioRestart != null)
            {
                sourceRestart.PlayOneShot(audioRestart);
            }
            Invoke(nameof(RestartLevel), loadDelay);
        }

        float move = 0f;

        if(Keyboard.current != null)
        {
            if(Keyboard.current[left].isPressed)
            {
                _spriteRender.flipX = true;
                move = 1f;
                isRunning = true;
            }

            else if(Keyboard.current[right].isPressed)
            {
                _spriteRender.flipX = false;
                move = -1f;
                isRunning = true;
            }
            else
            {
                 isRunning = false;
                 move = 0f;

            }
        }

        rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);

        if(animator != null)
        {
            animator.SetBool("isRunning", isRunning);
        }
    }
}
