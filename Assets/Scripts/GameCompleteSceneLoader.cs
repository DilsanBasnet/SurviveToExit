
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameCompleteSceneLoader : MonoBehaviour
{
  [SerializeField] private GameObject levelCompletePanel;
  [SerializeField] private TextMeshProUGUI levelTimeText;
  [SerializeField] private Image[] starImages;
  [SerializeField] private Sprite fillStarSprite;
  [SerializeField] private Sprite unfillStarSprite;

  [SerializeField] private float targetTimeForFiveStars = 30f;
  [SerializeField] private AudioClip buttonClickSfx;

  private AudioSource audioSource;

  private bool isCompleted = false;

 
  void Awake()
    {
       audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();

        if (levelCompletePanel != null) levelCompletePanel.SetActive(false);
    }

    public void TriggerLevel20Complete(float levelTime)
    {
        if(isCompleted ) return;
        isCompleted = true;

        Time.timeScale = 0f;

        if(levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(true);
        }

        int minutes = Mathf.FloorToInt(levelTime / 60f);
        int seconds = Mathf.FloorToInt(levelTime % 60f);

        if(levelTimeText != null)
        {
            levelTimeText.text = $"TIME : {minutes: 00} : {seconds:00}";
        }

        int earnedStars = CalculateStars(levelTime);
        DisplayStars(earnedStars);
    }

    private int CalculateStars(float timeTaken)
    {
        if(timeTaken <= targetTimeForFiveStars) return 5;
        if(timeTaken <= targetTimeForFiveStars * 1.25f) return 4;
        if(timeTaken <= targetTimeForFiveStars * 1.5f) return 3;
        if(timeTaken <= targetTimeForFiveStars * 2.0f) return 2;
        return 1;
    }
    private void DisplayStars(int starsCount)
    {
      for (int i = 0; i < starImages.Length; i++)
        {
            if (starImages[i] != null)
            {
                starImages[i].sprite = (i < starsCount) ? fillStarSprite : unfillStarSprite;
            }
        }
    }

    public void OnGoToGameCompleteSceneClicked()
    {
       StartCoroutine(LoadSceneRoutine("GameCompleteScene"));
    }

    public void OnMainMenuClicked()
    {
        StartCoroutine(LoadSceneRoutine("MainMenuScene"));
    }

    private System.Collections.IEnumerator LoadSceneRoutine(string sceneName)
    {
        if(buttonClickSfx != null && audioSource != null)
        {
            audioSource.PlayOneShot(buttonClickSfx);
        }

        yield return new WaitForSecondsRealtime(0.15f);
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

  }
