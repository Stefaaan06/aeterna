using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class fadeIn : MonoBehaviour
{
    public Image panel;


    private void Start()
    {

        fadeToNormal();
    } 

    public void fadeToNormal()
    {
        StartCoroutine(FadePanel(8, 0f)); 
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
