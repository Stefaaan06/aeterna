using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public fadeIn fade;
    public mainMenuAnim anim;

    public AudioSource musicSource;

    private int lvl = 1;
    public void StartGame()
    {
        fade.fadeToBlack();
        StartCoroutine(FadeOutMusic(1f));
        Invoke("start", 1f);
    }
    
    public void continuegame()
    {
        lvl = PlayerPrefs.GetInt("lvl");
        fade.fadeToBlack();
        StartCoroutine(FadeOutMusic(1f));
        Invoke("start", 1f);
    }
    
    private void start()
    {
        SceneManager.LoadScene(lvl);
    }

    IEnumerator FadeOutMusic(float fadeOutDuration)
    {
        float startVolume = musicSource.volume;

        while (musicSource.volume > 0)
        {
            musicSource.volume -= startVolume * Time.deltaTime / fadeOutDuration;
            yield return null;
        }

        musicSource.volume = 0;
    }
    
    public void Settings()
    {
        StartCoroutine(anim.fadeOutMain());
        anim.fadeInSettings();
    }
    
    public void settingsBack()
    {
        StartCoroutine(anim.fadeOutSettings());
        anim.fadeInMain();
    }
    
    public void Credits()
    {
        StartCoroutine(anim.fadeOutMain());
        anim.fadeInCredits();
    }
    
    public void creditsBack()
    {
        StartCoroutine(anim.fadeOutCredits());
        anim.fadeInMain();
    }
    public void openURL(String url)
    {
        Application.OpenURL(url);
    }
    public void Quit()
    {
        
        fade.fadeToBlack();
        Invoke("QuitGame", 1f);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("quit");
    }
}
