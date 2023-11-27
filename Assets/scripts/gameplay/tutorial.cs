using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class tutorial : MonoBehaviour
{
    public TextMeshProUGUI[] tutorialText;


    public void fadeInText(int num)
    {
        Debug.Log("start");

        StartCoroutine(fadeInText(tutorialText[num], 1f));
        StartCoroutine(waitForFadeOut(num));
    }
    
    public void fadeOuText(int num)
    {
        StartCoroutine(fadeInText(tutorialText[num], 0f));
    }

    IEnumerator waitForFadeOut(int num)
    {
        yield return new WaitForSeconds(4);
        fadeOuText(num);
    }
    
    IEnumerator fadeInText(TextMeshProUGUI tmp, float targetAlpha)
    {
        Debug.Log("start 2   ");
        Color startColor = tmp.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, targetAlpha); 
        float duration = 2f; 
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
