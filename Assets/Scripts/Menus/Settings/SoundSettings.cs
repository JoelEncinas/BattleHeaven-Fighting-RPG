using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SoundSettings : MonoBehaviour
{
    // UI
    Slider sliderSounds;
    Slider sliderMusic;
    Image soundOn;
    Image soundOff;
    Image musicOn;
    Image musicOff;

    private void Awake()
    {
        InitUI();
        SetupUI();
    }

    private void InitUI()
    {
        // UI
        sliderSounds = GameObject.Find("SliderSounds").GetComponent<Slider>();
        sliderMusic = GameObject.Find("SliderMusic").GetComponent<Slider>();
        soundOn = GameObject.Find("SoundOn").GetComponent<Image>();
        soundOff = GameObject.Find("SoundOff").GetComponent<Image>();
        musicOn = GameObject.Find("MusicOn").GetComponent<Image>();
        musicOff = GameObject.Find("MusicOff").GetComponent<Image>();

        // onValueChange
        sliderSounds.onValueChanged.AddListener(SoundListener);
        sliderMusic.onValueChanged.AddListener(MusicListener);
    }

    private void SetupUI()
    {
        sliderMusic.value = PlayerPrefs.GetFloat("musicVolume");
        sliderSounds.value = PlayerPrefs.GetFloat("soundsVolume");
        MusicListener(PlayerPrefs.GetFloat("musicVolume"));
        SoundListener(PlayerPrefs.GetFloat("soundsVolume"));
    }

    public void SoundListener(float value)
    {
        if (value == 0)
        {
            FindObjectOfType<AudioManager>().ChangeVolume("S", value / 10);
            soundOn.enabled = false;
            soundOff.enabled = true;
        }
        else
        {
            FindObjectOfType<AudioManager>().ChangeVolume("S", value / 10);
            soundOn.enabled = true;
            soundOff.enabled = false;
        }

        SaveMusicPrefs("sounds", value);
    }

    // Slider scale is 0-10 and audio is 0-1
    public void MusicListener(float value)
    {
        if (value == 0)
        {
            FindObjectOfType<AudioManager>().ChangeVolume("V", value / 10);
            musicOn.enabled = false;
            musicOff.enabled = true;
        }
        else
        {
            FindObjectOfType<AudioManager>().ChangeVolume("V", value / 10);
            musicOn.enabled = true;
            musicOff.enabled = false;
        }

        SaveMusicPrefs("music", value);
    }

    private void SaveMusicPrefs(string setting, float value)
    {
        switch (setting)
        {
            case "music":
                PlayerPrefs.SetFloat("musicVolume", value);
                PlayerPrefs.Save();
                break;
            case "sounds":
                PlayerPrefs.SetFloat("soundsVolume", value);
                PlayerPrefs.Save();
                break;
        }
    }
}
