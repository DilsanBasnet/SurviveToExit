using UnityEngine;
using UnityEngine.SceneManagement;

public class LaserDamage : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>() ;
            if(player != null)
            {
                player.TriggerPlayerCameraShake(3f, 0.3f);
              player.TriggerExternalDeath();
            }
            else
            {
                RestartScene();
            }
        }

        else if (collision.CompareTag("Rocket"))
        {
            rocketController rocket = collision.GetComponent<rocketController>();

            if(rocket != null)
            {
                rocket.TriggerRocketCameraShake(4.5f, 0.4f);
                rocket.TriggerExternalCrash();
            
            }
            else
            {
                RestartScene();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if(player != null)
            {
                player.TriggerPlayerCameraShake(3f, 0.3f);
                player.TriggerExternalDeath();
            }
             else
            {
                RestartScene();
            }
        }
        else if ( collision.gameObject.CompareTag("Rocket"))
        {
           rocketController rocket = collision.gameObject.GetComponent<rocketController>();

           if(rocket != null)
            {
                rocket.TriggerRocketCameraShake(4.5f, 0.4f);
                rocket.TriggerExternalCrash();
            }
            else
            {
                RestartScene();
                
            }
        }
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
