using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGoal : MonoBehaviour
{
  public int currentLevelNumber = 1;

  public string levelSelectSceneName = "LevelsScene";

  private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") || collision.CompareTag("Rocket"))
        {
            LevelSelectManager.CompleteLevel(currentLevelNumber);

            SceneManager.LoadScene(levelSelectSceneName); 
        }
    }
}
