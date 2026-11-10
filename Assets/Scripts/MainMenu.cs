using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public AudioClip buttonClickSfx;
    private AudioSource audioSource;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if(audioSource == null)
        {
        
                audioSource = gameObject.AddComponent<AudioSource>(); 
        }
    }

    public void PlayGame()
    {
        StartCoroutine(PlaySoundAndLoad("Level1"));
    }
        public void OpenLevels()
    {
        StartCoroutine(PlaySoundAndLoad("LevelsScene"));

    }

    public void QuitGame()
    {
       StartCoroutine(PlaySoundAndQuit());
    }

    private IEnumerator PlaySoundAndLoad(string sceneName)
    {
        if(buttonClickSfx != null && audioSource != null)
        {
            audioSource.PlayOneShot(buttonClickSfx);
        }

        yield return new WaitForSecondsRealtime(0.2f);

        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator PlaySoundAndQuit()
    {
        if(buttonClickSfx != null && audioSource != null)
        {
            audioSource.PlayOneShot(buttonClickSfx);
        }

        yield return new WaitForSecondsRealtime(0.2f);

        Application.Quit();
    }
}
