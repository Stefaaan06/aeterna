using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class fadeIn : MonoBehaviour
{
    public Image panel;


    private void Start()
    {

        fadeToNormal();
    } 

    public void fadeToBlack()
    {
        StopAllCoroutines();
        StartCoroutine(FadePanel(1.5f, 1f)); 
    }
    
    public void fadeToNormal()
    {
        StopAllCoroutines();
        StartCoroutine(FadePanel(4, 0f)); 
    }
    
    private IEnumerator FadePanel(float duration, float targetAlpha)
    {
        Color startColor = panel.color;
        Color targetColor = new Color(0, 0, 0, targetAlpha);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            panel.color = Color.Lerp(startColor, targetColor, elapsed / duration);

            yield return null;

            elapsed += Time.deltaTime;
        }

        panel.color = targetColor;
    }
}
