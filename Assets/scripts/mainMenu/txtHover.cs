using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class txtHover : MonoBehaviour
{
    public Image panel;
    public TMP_Text text;

    private Color originalTextColor = Color.black;
    private Color hoverTextColor = Color.white;

    private void Start()
    {
        EventTrigger eventTrigger = text.gameObject.GetComponent<EventTrigger>();
        if (eventTrigger == null)
        {
            eventTrigger = text.gameObject.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry entryEnter = new EventTrigger.Entry();
        entryEnter.eventID = EventTriggerType.PointerEnter;
        entryEnter.callback.AddListener((data) => { OnPointerEnter(); });
        eventTrigger.triggers.Add(entryEnter);

        EventTrigger.Entry entryExit = new EventTrigger.Entry();
        entryExit.eventID = EventTriggerType.PointerExit;
        entryExit.callback.AddListener((data) => { OnPointerExit(); });
        eventTrigger.triggers.Add(entryExit);
    }

    private void OnPointerEnter()
    {
        text.color = hoverTextColor;

        StartCoroutine(FadePanel(true));
    }

    private void OnPointerExit()
    {
        text.color = originalTextColor;

        StartCoroutine(FadePanel(false));
    }

    private IEnumerator FadePanel(bool fadeIn)
    {
        float targetAlpha = fadeIn ? 1.0f : 0.0f;
        float currentAlpha = panel.color.a;
        float fadeSpeed = 5.0f; 

        while (!Mathf.Approximately(currentAlpha, targetAlpha))
        {
            currentAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, fadeSpeed * Time.deltaTime);
            Color newColor = panel.color;
            newColor.a = currentAlpha;
            panel.color = newColor;

            yield return null;
        }
        panel.color = new Color(0, 0, 0, targetAlpha);
    }
}
