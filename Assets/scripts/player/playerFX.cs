using UnityEngine.UI;
using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

//manages player vfx, sfx and other effects
public class playerFX : MonoBehaviour
{
    public Image panel;
    

    private void Start()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Level 1"))
        {
            Debug.Log("x");
            fadeToNormalLong();
        }
        else
        {
            fadeToNormal();
        }
    }

    
    public void shakeEarthquake()
    {
        CameraShaker.CameraShaker.Instance.Shake(CameraShaker.CameraShakePresets.Earthquake);
    }
    public void fadeToNormalLong()
    {
        StartCoroutine(FadePanel(20, 0f)); 

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

