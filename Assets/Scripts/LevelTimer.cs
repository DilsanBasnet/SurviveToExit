using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelTimer : MonoBehaviour
{
    
    public TMP_Text timerText;
    public GameObject timeOutPanel;

    

    public float LevelTimeLimit = 60f;

    private float remainingTime;
    private bool isTiming = true;

    void Start()
    {
        remainingTime = LevelTimeLimit;
        isTiming = true;
        Time.timeScale = 1f;

        if(timeOutPanel != null)
        {
            timeOutPanel.SetActive(false);
        }
    }



    void Update()
    {
        if(isTiming)
        {
            if(remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;

                if(timerText != null)
                {
                    int minutes = Mathf.FloorToInt(remainingTime / 60f); 
                    int seconds = Mathf.FloorToInt(remainingTime % 60f);
                    timerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
                }

                if(remainingTime <= 0)
                {
                    remainingTime = 0;
                    isTiming = false;
                    TriggerTimeOut();            
              }
            }
        }
    }
    public void StopTimerAndSave()
    {
        if(!isTiming) return;
        isTiming = false;    
    }

    void TriggerTimeOut()
    {
      if(timeOutPanel != null)
        {
            timeOutPanel.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    public void PlayAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
