using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections;


public class PauseManager : MonoBehaviour
{
   public GameObject pausePanel;
   private bool isPaused = false;

   public AudioClip buttonClickSfx;
   private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if(audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        if(pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
    }

    void Update()
    {
        if(Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if(isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        if(pausePanel != null)
        {
          pausePanel.SetActive(true);   
        }

        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        StartCoroutine(PlayClickAndResume());
    }


    public void GoToMainMenu()
    {
        StartCoroutine(PlayClickAndLoad("MainMenuScene"));
            }

    public void QuitGame()
    {
        StartCoroutine(PlayClickAndQuit());
    }

    private IEnumerator PlayClickAndResume()
    {
        if(buttonClickSfx != null && audioSource != null)
        {
            audioSource.PlayOneShot(buttonClickSfx);
        }
        yield return new WaitForSecondsRealtime(0.15f);
        isPaused = false;
        if(pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
        Time.timeScale = 1f;

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
