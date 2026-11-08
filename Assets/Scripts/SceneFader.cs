using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
  public static SceneFader Instance;
  public Image fadeImage;
  public float fadeSpeed = 1.5f;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
         else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if(fadeImage != null)
        {
            StartCoroutine(FadeInRoutine());
        }
    }

    public void FadeToLevel(string sceneName)
    {
        StartCoroutine(FadeOutRoutine(sceneName));
    }

    private IEnumerator FadeOutRoutine(string sceneName)
    {
        if(fadeImage == null) yield break;

        fadeImage.gameObject.SetActive(true);
        float alpha = 0f;

        while (alpha < 1f)
        {
            alpha += Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator FadeInRoutine()
    {
        float alpha = 1f;
        fadeImage.color = new Color(0f, 0f, 0f, 1f);

        while (alpha > 0f)
        {
            alpha -= Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }

        fadeImage.gameObject.SetActive(false);
    }
}
