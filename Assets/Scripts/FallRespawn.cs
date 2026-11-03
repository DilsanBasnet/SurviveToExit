using System.Collections;
using UnityEngine;

public class FallRespawn : MonoBehaviour
{
   public Transform respawnPoint;
   
   public float respawnDelay = 1f;
   private bool isRespawing = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DeathZone") && !isRespawing)
        {
            StartCoroutine(RespawnCoroutine());
        }


    }

    private IEnumerator RespawnCoroutine()
    {
       isRespawing = true;

       Rigidbody2D rb = GetComponent<Rigidbody2D>();

       if(rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
        }

        yield return new WaitForSeconds(respawnDelay);

        if(respawnPoint != null)
        {
            transform.position = respawnPoint.position;
        }


        if(rb != null)
        {
            rb.simulated = true;

        }
        isRespawing = false;
    }
}
