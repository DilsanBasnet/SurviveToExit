using UnityEngine;
using UnityEngine.SceneManagement;

public class RotatingObstacle : MonoBehaviour
{
    public float rotationSpeed = 100f;

    void Update()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }


    private void OisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Rocket"))
        {
            RestartGame();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") || collision.CompareTag("Rocket"))
        {
            RestartGame();
        }
    }

    private void RestartGame()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
