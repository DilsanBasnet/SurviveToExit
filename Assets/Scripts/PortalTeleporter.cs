using System.Collections;
using UnityEngine;

public class PortalTeleporter : MonoBehaviour
{
    public Transform targetPortal;
    private static bool canTeleport = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {

      if(!canTeleport) return;

      if(collision.CompareTag("Player") || collision.GetComponent<Player>() != null)
        {
            if(targetPortal != null)
            {
                StartCoroutine(TeleportCooldown());

                collision.transform.position = targetPortal.position;

                Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();

                if(rb != null)
                {
                    
                }
            }
        }  
    }

    private IEnumerator TeleportCooldown()
    {
        canTeleport = false;

        yield return new WaitForSeconds(0.3f);
        canTeleport = true;
    }
}
