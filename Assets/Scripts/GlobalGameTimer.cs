using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalGameTimer : MonoBehaviour
{
    public static GlobalGameTimer Instance;

    public float totalElapsedTime = 0f;
    public bool isTimerRunning = false;

    public  TMP_Text globalTimerTextUI;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
            return;
        }
    }


    void Update()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if(currentScene.StartsWith("Level"))
        {
            isTimerRunning = true;
            totalElapsedTime += Time.unscaledDeltaTime;
        }

        else
        {
            isTimerRunning = false;
        }

        if(globalTimerTextUI == null)
        {
            GameObject textObj = GameObject.Find("TIMERTEXT");

            if(textObj != null)
            {
                globalTimerTextUI = textObj.GetComponent<TMP_Text>();
            }
        }

        UpdateUI();
    }

    public void ResetTimer()
    {
        totalElapsedTime = 0f;
        isTimerRunning = true;

    }

    private void UpdateUI()
    {
        if(globalTimerTextUI != null)
        {
            int minutes = Mathf.FloorToInt(totalElapsedTime / 60f);
            int seconds = Mathf.FloorToInt(totalElapsedTime % 60f);
            int milliseconds = Mathf.FloorToInt((totalElapsedTime * 100f) % 100f);

            globalTimerTextUI.text = string.Format("TOTAL : {0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
        }
    }
}
