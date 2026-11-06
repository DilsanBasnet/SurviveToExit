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
                RestartGame();
            }
        }
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
