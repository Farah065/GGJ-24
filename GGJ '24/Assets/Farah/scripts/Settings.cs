using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    public AudioMixer master;

    public Dropdown resolution;
    public Toggle fullScreen;
    public Dropdown graphics;
    public Slider masterVolume;
    public Slider musicVolume;
    public Slider sfxVolume;

    Resolution[] resolutions;
    void Start()
    {
        // adding possible resolution values to the dropdown options
        resolutions = Screen.resolutions;
        resolution.ClearOptions();
        List<string> options = new List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
        }

        resolution.AddOptions(options);

        // displaying the correct values for the UI stuff
        resolution.value = PlayerPrefs.GetInt("Resolution", MainMenu.currentResolutionIndex);
        resolution.RefreshShownValue();

        fullScreen.isOn = (PlayerPrefs.GetInt("FullScreen", 1) != 0);
        graphics.value = PlayerPrefs.GetInt("Quality", 2);
        masterVolume.value = PlayerPrefs.GetFloat("MasterVolume", 1);
        musicVolume.value = PlayerPrefs.GetFloat("MusicVolume", 1);
        sfxVolume.value = PlayerPrefs.GetFloat("sfxVolume", 1);
    }
    public void SetResolution(int resolutionIndex)
    {
        resolutionIndex = resolution.value;
        MainMenu.currentResolutionIndex = resolutionIndex;
        PlayerPrefs.SetInt("Resolution", resolutionIndex);
        Resolution res = resolutions[resolutionIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreenMode);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        PlayerPrefs.SetInt("FullScreen", (isFullScreen ? 1 : 0));
        Screen.fullScreen = isFullScreen;
    }

    public void SetQuality(int qualityIndex)
    {
        PlayerPrefs.SetInt("Quality", qualityIndex);
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("Quality", 2), true);
    }

    public void SetVolume(float volume)
    {
        PlayerPrefs.SetFloat("MasterVolume", volume);
        float vol = Mathf.Log10(PlayerPrefs.GetFloat("MasterVolume", 1)) * 20;
        master.SetFloat("MasterVolume", vol);
    }

    public void SetMusic(float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", volume);
        float vol = Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume", 1)) * 20;
        master.SetFloat("MusicVolume", vol);
    }

    public void SetSFX(float volume)
    {
        PlayerPrefs.SetFloat("sfxVolume", volume);
        float vol = Mathf.Log10(PlayerPrefs.GetFloat("sfxVolume", 1)) * 20;
        master.SetFloat("sfxVolume", vol);
    }

    public void BackToGame()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
