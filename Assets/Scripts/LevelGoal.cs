using System.Collections;
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

            AudioSource rocketAudio = collision.GetComponent<AudioSource>();

            if(rocketAudio == null)
            {
                rocketAudio = collision.GetComponentInChildren<AudioSource>();
            }

            GameObject rocketObj = GameObject.FindGameObjectWithTag("Rocket");

            if(rocketObj != null)
            {
                rocketController controller = rocketObj.GetComponent<rocketController>();

                if(controller != null && controller.successfullLandingSfx != null)
                {
                    rocketAudio = rocketObj.GetComponent<AudioSource>();

                    if(rocketAudio == null) rocketAudio = rocketObj.AddComponent<AudioSource>();

                    rocketAudio.PlayOneShot(controller.successfullLandingSfx);
                }
            }

            StartCoroutine(WaitAndLoadNextLevel());

        }
        
    }

    private IEnumerator WaitAndLoadNextLevel()
    {
        yield return new WaitForSeconds(1.5f);

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
