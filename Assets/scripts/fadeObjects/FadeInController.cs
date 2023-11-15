using UnityEngine;

public class FadeInController : MonoBehaviour
{
    private float _currentAlpha;
    public void StartFadeIn(Material material, int fadeDuration)
    {
        StartCoroutine(FadeIn(material, fadeDuration));
    }

    private System.Collections.IEnumerator FadeIn(Material material, int fadeDuration)
    {
        _currentAlpha = 0;
        float elapsedTime = 0f;
        Color color = material.color;

        while (elapsedTime < fadeDuration)
        {
            _currentAlpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            color.a = _currentAlpha;
            material.color = color;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        color.a = 1f;
        material.color = color;
    }
}