using UnityEngine;
using UnityEngine.UI;

public class VolumeSider : MonoBehaviour
{
    private Slider volumeSlider;

    void Start()
    {
        volumeSlider = GetComponent<Slider>();

        if(AudioManager.Instance != null && volumeSlider != null)
        {
            volumeSlider.value = AudioManager.Instance.musicVolume;
        }

        if(volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(SetMusicVolume);
        }
    }

    public void SetMusicVolume(float value)
    {
        if(AudioManager.Instance != null)
        {
            AudioManager.Instance.musicVolume = value;
        }
    }
}
