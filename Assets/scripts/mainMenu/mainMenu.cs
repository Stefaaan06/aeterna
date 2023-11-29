using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public fadeIn fade;
    public mainMenuAnim anim;

    public void StartGame()
    {
        fade.fadeToBlack();
        Invoke("start", 1f);
    }

    private void start()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
