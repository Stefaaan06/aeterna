using UnityEngine.UI;
using UnityEngine;
using System;
using System.Collections;

//manages player vfx, sfx and other effects
public class playerFX : MonoBehaviour
{
    public Image panel;

    private void Start()
    {
        fadeToNormal();
    }

    public void shakeEarthquake()
    {
        CameraShaker.CameraShaker.Instance.Shake(CameraShaker.CameraShakePresets.Earthquake);
    }

    public void fadeToNormal()
    {
        StartCoroutine(FadePanel(6, 0f)); 
    }
    
    public void fadeToBlack()
    {
        StartCoroutine(FadePanel(4, 1f)); 
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

