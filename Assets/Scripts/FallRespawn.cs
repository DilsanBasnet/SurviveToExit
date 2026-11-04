using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FallRespawn : MonoBehaviour
{
   private bool isRestarting = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((collision.CompareTag("DeathZone") || collision.gameObject.CompareTag("Deathzone")) && !isRestarting)
        {
           isRestarting = true;
           SceneManager.LoadScene(SceneManager.GetActiveScene().name) ; 
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("DeathZone") && !isRestarting)
        {
            isRestarting = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
