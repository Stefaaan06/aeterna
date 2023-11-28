using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class mainMenuStart : MonoBehaviour
{
    public TMP_Text[] textStart;
    public TMP_Text[] otherText;
    public GameObject otherTextObject;
    void Update()
    {
        if (Input.anyKey)
        {
            foreach (TMP_Text text in textStart)
            {
                fadeOuText(text, 0, 1);
            }
            otherTextObject.SetActive(true);
            foreach (TMP_Text text in otherText)
            {
                fadeInText(text, 1, 0.5f);
            }

            this.enabled = false;
        }
    }

    public void fadeInText(TMP_Text text, float delay, float time)
    {
        StartCoroutine(fadeText(text, 1f, delay, time));
    }

    public void fadeOuText(TMP_Text text, float delay, float time)
    {
        StartCoroutine(fadeText(text, 0f, delay, time));
    }
    
    
    IEnumerator fadeText(TMP_Text tmp, float targetAlpha, float delay, float time)
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
