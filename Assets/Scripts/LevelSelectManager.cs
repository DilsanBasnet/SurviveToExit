using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectManager : MonoBehaviour
{
    [System.Serializable]
       public struct LevelButtonInfo
    {
        public Button buttonComponent;
        public GameObject statusTagObject;
        public GameObject playerSprite;
        public TMP_Text levelText;
        public int levelNumber;
        
    }

    public GameObject panel1;
    public GameObject panel2;

    public LevelButtonInfo[] levels;
    public Sprite unlockedSprite;
    public Sprite unlockedTagSprite;
    public Sprite lockedTagSprite;

    public Sprite lockedSprite;

    public AudioClip buttonClickSfx;
    private AudioSource audioSource;

    private const string PREF_KEY = "LevelUnlocked";

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if(audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Start()
    {
        if(!PlayerPrefs.HasKey(PREF_KEY))
        {
            PlayerPrefs.SetInt(PREF_KEY, 1);
            PlayerPrefs.Save();
        }

        UpdateLevelStates();

        OpenPanel1();
    }

    public void OpenPanel2()
    {
        if(buttonClickSfx != null && audioSource != null) audioSource.PlayOneShot(buttonClickSfx);
        if(panel1 != null ) panel1.SetActive(false);
        if(panel2 != null) panel2.SetActive(true);

    }

    public void OpenPanel1()
    {
        if(buttonClickSfx != null && audioSource != null) audioSource.PlayOneShot(buttonClickSfx);
        if(panel2 != null) panel2.SetActive(false);
        if(panel1 != null) panel1.SetActive(true);
    }

    public void UpdateLevelStates()
    {
        int unlockedLevel = PlayerPrefs.GetInt(PREF_KEY, 1) ;

        for (int i = 0; i < levels.Length; i++)
        {
            int currentLevelNum = levels[i].levelNumber;
            Image buttonImage = levels[i].buttonComponent != null ? levels[i].buttonComponent.GetComponent<Image>() : null;
            Image tagImage = levels[i].statusTagObject != null ? levels[i].statusTagObject.GetComponent<Image>() : null;

            if(currentLevelNum <= unlockedLevel)
            {
                if(levels[i].buttonComponent != null)

               levels[i].buttonComponent.interactable = true;

               if(buttonImage != null && unlockedSprite != null)
                {
                    buttonImage.sprite = unlockedSprite;
                } 

                if(levels[i].statusTagObject != null)
                {
                    levels[i].statusTagObject.SetActive(true);

                    if(tagImage != null && unlockedTagSprite != null )
                    {
                        tagImage.sprite = unlockedTagSprite;
                    }
                }

                if(levels[i].playerSprite != null)
                {
                    levels[i].playerSprite.SetActive(currentLevelNum == unlockedLevel);
                }
            }
            else
            {
                if(levels[i].buttonComponent != null)
                levels[i].buttonComponent.interactable = false;
                
                if(buttonImage != null && lockedSprite != null)
                {
                    buttonImage.sprite = lockedSprite;
                }

                if(levels[i].statusTagObject != null)
                {
                    levels[i].statusTagObject.SetActive(true);

                    if(tagImage != null && lockedTagSprite != null)
                    {
                        tagImage.sprite = lockedTagSprite;
                    }
                }

                if(levels[i].playerSprite != null)
                {
                    levels[i].playerSprite.SetActive(false);
                }


            }
        }
    }

    public void LoadLevel(int levelNum)
    {
        StartCoroutine(PlaySoundAndLoadLevel(levelNum));
    }

     private IEnumerator PlaySoundAndLoadLevel(int levelNum)
    {
        if(buttonClickSfx != null && audioSource != null)
        {
            audioSource.PlayOneShot(buttonClickSfx);
        }

        yield return new WaitForSecondsRealtime(0.15f);

        SceneManager.LoadScene("Level" + levelNum);
    }

    public static void CompleteLevel(int levelCompleted)
    {
        int currentUnlocked = PlayerPrefs.GetInt(PREF_KEY, 1);

        if(levelCompleted >= currentUnlocked)
        {
            PlayerPrefs.SetInt(PREF_KEY, levelCompleted + 1);
            PlayerPrefs.Save();
        }
    }

    public void BackToMainMenu()
    {
        StartCoroutine(PlaySoundAndLoad("MainMenuScene"));

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

        yield return new WaitForSecondsRealtime(0.15f);
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator PlaySoundAndQuit()
    {
        if(buttonClickSfx != null && audioSource != null)
        {
            audioSource.PlayOneShot(buttonClickSfx);
        }
        yield return new WaitForSecondsRealtime(0.15f);

        Application.Quit();

    }
}
