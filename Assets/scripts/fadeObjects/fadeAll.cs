using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fadeAll : MonoBehaviour
{
    public float fadeDuration = 2f;
    public float startAlpha;
    private Material _material;
    
    private float _currentAlpha;
    private Collider _col;
    void Awake()
    {
        _col = this.GetComponent<Collider>();
        Renderer renderer = this.GetComponent<Renderer>();

        _material = renderer.material;

        if (startAlpha == 0)
        {
            _col.enabled = false;
        }
        else
        {
            _col.enabled = true;
        }
        renderer.material = _material;
    }

    public void fadeIn()
    {
        StopAllCoroutines();
        StartCoroutine(FadeIn());
        _col.enabled = true;
    }
    
    private System.Collections.IEnumerator FadeIn()
    {
        _currentAlpha = 0;
        float elapsedTime = 0f;
        Color color = _material.color;

        while (elapsedTime < fadeDuration)
        {
            _currentAlpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            color.a = _currentAlpha;
            _material.color = color;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        color.a = 1f;
        _material.color = color;
    }

    public void fadeOut()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutCoroutine());
    }
    
    private System.Collections.IEnumerator FadeOutCoroutine()
    {
        _currentAlpha = 1f;
        float elapsedTime = 0f;
        Color color = _material.color;

        while (elapsedTime < fadeDuration)
        {
            _currentAlpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            color.a = _currentAlpha;
            _material.color = color;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        color.a = 0f;
        _material.color = color;
        _col.enabled = false;
    }
}
