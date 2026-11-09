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

    public LevelButtonInfo[] levels;
    public Sprite unlockedSprite;
    public Sprite unlockedTagSprite;
    public Sprite lockedTagSprite;

    public Sprite lockedSprite;

    private const string PREF_KEY = "LevelUnlocked";

    void Start()
    {
        if(!PlayerPrefs.HasKey(PREF_KEY))
        {
            PlayerPrefs.SetInt(PREF_KEY, 1);
            PlayerPrefs.Save();
        }

        UpdateLevelStates();
    }

    void UpdateLevelStates()
    {
        int unlockedLevel = PlayerPrefs.GetInt(PREF_KEY, 1) ;

        for (int i = 0; i < levels.Length; i++)
        {
            int currentLevelNum = levels[i].levelNumber;
            Image buttonImage = levels[i].buttonComponent.GetComponent<Image>();
            Image tagImage = levels[i].statusTagObject != null ? levels[i].statusTagObject.GetComponent<Image>() : null;

            if(currentLevelNum <= unlockedLevel)
            {
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
        SceneManager.LoadScene("MainMenuScene");

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
