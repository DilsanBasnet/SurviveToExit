using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGoal : MonoBehaviour
{
  public int currentLevelNumber = 1;


  private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") || collision.CompareTag("Rocket"))
        {
            LevelSelectManager.CompleteLevel(currentLevelNumber);

            int nextLevelNum = currentLevelNumber + 1;
            string nextSceneName = "Level" + nextLevelNum;

            if(Application.CanStreamedLevelBeLoaded(nextSceneName))
            {
                SceneManager.LoadScene(nextSceneName);
            }

            else
            {
                SceneManager.LoadScene("LevelsScene");
            }
            
        }
        
    }

}
