using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.UI;
public class settingsMenu : MonoBehaviour
{
    public Slider volume;
    public AudioMixer mixer;
    public GameObject[] toggleImages;


    void Awake()
    {
        if (PlayerPrefs.GetInt("Start") == 0)
        {
            PlayerPrefs.SetInt("lvl", 1);
            PlayerPrefs.SetInt("quality", 1);
            PlayerPrefs.SetInt("mainVolume ", 0);
            PlayerPrefs.SetInt("Start", 1);
        }
    } 

    public void UpdateValues()
    {
        mixer.SetFloat("mainVolume", volume.value);
        PlayerPrefs.SetFloat("mainVolume", volume.value);
    }

    private void Start()
    {
        volume.value = PlayerPrefs.GetFloat("mainVolume");
        mixer.SetFloat("mainVolume", PlayerPrefs.GetFloat("mainVolume"));
        
        Time.timeScale = 1f;

        int cameraShake = PlayerPrefs.GetInt("cameraShake");
        if (cameraShake == 1)
        {
            toggleImages[0].SetActive(false);
        }
        else
        {
            toggleImages[0].SetActive(true);
        }


        int quality = PlayerPrefs.GetInt("quality");
        switch (quality)
        {
            case 0:
                toggleImages[1].SetActive(true);
                QualitySettings.SetQualityLevel(0);
                break;
            case 1:
                toggleImages[2].SetActive(true);
                QualitySettings.SetQualityLevel(1);
                break;
            case 2:
                toggleImages[3].SetActive(true);
                QualitySettings.SetQualityLevel(2);
                break;
            default:
                toggleImages[1].SetActive(true);
                QualitySettings.SetQualityLevel(1);
                PlayerPrefs.SetInt("quality", 1);
                break;
        }
    }
    

    public void cameraShake()
    {
        if (toggleImages[0].activeSelf)
        {
            toggleImages[0].SetActive(false);
            PlayerPrefs.SetInt("cameraShake", 1);
        }
        else
        {
            toggleImages[0].SetActive(true);
            PlayerPrefs.SetInt("cameraShake", 0);
        }
    }
    

    public void setQuality(int lvl)
    {
        QualitySettings.SetQualityLevel(lvl);
        PlayerPrefs.SetInt("quality", lvl);
    }
}
