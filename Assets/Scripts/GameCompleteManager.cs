using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCompleteManager : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI totalTimeText;
   [SerializeField] private AudioClip buttonClickSfx;
   
   private AudioSource audioSource;

   void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if(audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        DisplayTotalTime();
    }


    private void DisplayTotalTime()
    {
        if(totalTimeText != null)
        {
            float finalTime = 0f;

            if(GlobalGameTimer.Instance != null)
            {
                finalTime = GlobalGameTimer.Instance.totalElapsedTime;
            }

            int minutes = Mathf.FloorToInt(finalTime / 60f);
            int seconds = Mathf.FloorToInt(finalTime % 60f);
            totalTimeText.text = $"TOTAL TIME : {minutes:00}:{seconds:00}";
        }
    }

    public void OnMainMenuClicked()
    {
        StartCoroutine(PlayClickAndLoad("MainMenuScene"));
    }

    public void OnQuitClicked()
    {
        StartCoroutine(PlayClickAndQuit());
    }

    private IEnumerator PlayClickAndLoad(string sceneName)
    {
        if(buttonClickSfx != null && audioSource != null)
        {
            audioSource.PlayOneShot(buttonClickSfx);
        }

        yield return new WaitForSecondsRealtime(0.15f);

        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator PlayClickAndQuit()
    {
        if(buttonClickSfx != null && audioSource != null)
        {
            audioSource.PlayOneShot(buttonClickSfx);
        }

        yield return new WaitForSecondsRealtime(0.15f);

        Time.timeScale = 1f;
        Application.Quit();
    }
}
