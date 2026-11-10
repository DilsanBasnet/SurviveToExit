using UnityEngine;
using UnityEngine.SceneManagement;

public class Obstacle : MonoBehaviour
{
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
                RestartGame();

            }   
        } 

        else if(collision.gameObject.CompareTag("Rocket"))
        {
            rocketController rocket = collision.gameObject.GetComponent<rocketController>();

            if(rocket == null)
            {
                
              rocket.TriggerRocketCameraShake(4.5f, 0.4f);
              rocket.TriggerExternalCrash();
            }
            else
            {
                  RestartGame();
            }
        }
        }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
          Player player = collision.gameObject.GetComponent<Player>();

          if(player != null)
            {
                player.TriggerPlayerCameraShake(3f, 0.3f);
                player.TriggerExternalDeath();
            }
            else
            {
                RestartGame();
            }
        }

        else if (collision.gameObject.CompareTag("Rocket"))
        {
            rocketController rocket = collision.gameObject.GetComponent<rocketController>();

            if(rocket == null)
            {
                rocket.TriggerRocketCameraShake(4.5f, 0.4f);
                rocket.TriggerExternalCrash();
            }
            else
            {
                RestartGame();
            }
        }
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
