using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FallRespawn : MonoBehaviour
{
   private bool isRestarting = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((collision.CompareTag("DeathZone") ) && !isRestarting)
        {
            isRestarting = true;

            Player player = GetComponent<Player> ();
            
            if(player != null)
            {
                player.TriggerPlayerCameraShake(3.5f, 0.3f);
                player.TriggerExternalDeath();
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
          }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("DeathZone") && !isRestarting)
        {
            isRestarting = true;

            Player player = GetComponent<Player>();

            if(player != null)
            {
                player.TriggerPlayerCameraShake(3.5f, 0.3f);
                player.TriggerExternalDeath();
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}
