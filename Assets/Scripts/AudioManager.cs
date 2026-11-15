using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
   public static AudioManager Instance;

   public AudioClip menuAndLevelSelectMusic;
   public AudioClip playerSideMusic;
   public AudioClip spaceMusic;


   public float musicVolume = 0.5f;

   private AudioSource musicSource;
   private bool isSpaceMusicPlaying = false;

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

        musicSource = GetComponent<AudioSource>();

        if(musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
        }

        musicSource.loop = true;
        musicSource.volume = musicVolume;
    }

    void Update()
    {
        if(musicSource != null)
        {
            musicSource.volume = musicVolume;
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;

        isSpaceMusicPlaying = false;


        if(sceneName == "MainMenuScene" || sceneName == "LevelsScene" || sceneName.Contains("LevelSelect"))
        {
            if(musicSource.clip != menuAndLevelSelectMusic || !musicSource.isPlaying)
            {
                musicSource.clip = menuAndLevelSelectMusic;
                musicSource.volume = musicVolume;
                musicSource.Play();
            }
        } 

        else if (sceneName.StartsWith("Level"))
        {
            if(musicSource.clip != playerSideMusic || !musicSource.isPlaying)
            {
                musicSource.clip =  playerSideMusic;
                musicSource.volume = musicVolume;
                musicSource.Play();
            }
        }
    }

    public void SwitchToSpaceMusic()
    {
        if(!isSpaceMusicPlaying && spaceMusic != null)
        {
            isSpaceMusicPlaying = true;
            musicSource.clip = spaceMusic;
            musicSource.volume = musicVolume;
            musicSource.Play();
        }
    }

    public void SwitchToPlayerMusic()
    {
        isSpaceMusicPlaying = false;

        if(playerSideMusic != null && musicSource.clip != playerSideMusic)
        {
            musicSource.clip = playerSideMusic;
            musicSource.volume = musicVolume;
            musicSource.Play();
        }
    }
}
