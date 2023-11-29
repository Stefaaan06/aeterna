using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class mainMenuAnim : MonoBehaviour
{
    public TextMeshProUGUI[] textStart;
    public TextMeshProUGUI[] mainText;
    public GameObject mainTextObject;
    public TextMeshProUGUI[] settingsText;
    public GameObject settingsTextObject;
    public TextMeshProUGUI[] creditsText;
    public GameObject creditsTextObject;

    private bool _start = false;
    void Update()
    {
        if (Input.anyKey && !_start)
        {
            foreach (TextMeshProUGUI text in textStart)
            {
                fadeOuText(text, 0, 1);
            }
            mainTextObject.SetActive(true);
            foreach (TextMeshProUGUI text in mainText)
            {
                fadeInText(text, 1, 0.5f);
            }

            _start = true;
        }
    }

    private float _wait = 0.2f;
    public void fadeInMain()
    {
        mainTextObject.SetActive(true);
        foreach (TextMeshProUGUI text in mainText)
        {
            fadeInText(text, 0, _wait);
        }
    }
    
    
    public IEnumerator fadeOutMain()
    {
        foreach (TextMeshProUGUI text in mainText)
        {
            fadeOuText(text, 0, _wait);
        }
        yield return new WaitForSeconds(_wait);
        mainTextObject.SetActive(false);
    }
    
    public void fadeInSettings()
    {
        settingsTextObject.SetActive(true);
        foreach (TextMeshProUGUI text in settingsText)
        {
            fadeInText(text, 0, _wait);
        }
    }

    public IEnumerator fadeOutSettings()
    {
        foreach (TextMeshProUGUI text in settingsText)
        {
            fadeOuText(text, 0, _wait);
        }

        yield return new WaitForSeconds(_wait);
        settingsTextObject.SetActive(false);
    }
    
    public void fadeInCredits()
    {
        creditsTextObject.SetActive(true);
        foreach (TextMeshProUGUI text in creditsText)
        {
            fadeInText(text, 0, _wait);
        }
    }
    
    public IEnumerator fadeOutCredits()
    {
        foreach (TextMeshProUGUI text in creditsText)
        {
            fadeOuText(text, 0, _wait);
        }
        yield return new WaitForSeconds(_wait);
        creditsTextObject.SetActive(false);

    }
    public void fadeInText(TextMeshProUGUI text, float delay, float time)
    {
        StartCoroutine(fadeText(text, 1f, delay, time));
    }

    public void fadeOuText(TextMeshProUGUI text, float delay, float time)
    {
        StartCoroutine(fadeText(text, 0f, delay, time));
    }
    
    
    IEnumerator fadeText(TextMeshProUGUI tmp, float targetAlpha, float delay, float time)
    {
        yield return new WaitForSeconds(delay);
        
        Color startColor = tmp.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, targetAlpha); 
        float duration = time; 
        float elapsed = 0f;

        while (elapsed < duration)
        {
            tmp.color = Color.Lerp(startColor, targetColor, elapsed / duration);

            yield return null;

            elapsed += Time.deltaTime;
        }

        tmp.color = targetColor;
    }
}
