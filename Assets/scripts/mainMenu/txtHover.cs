using System;
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

    private mainMenuSFX _sfx;

    private void Start()
    {
        _sfx = FindObjectOfType<mainMenuSFX>();
        
        EventTrigger eventTrigger = gameObject.GetComponent<EventTrigger>();
        if (eventTrigger == null)
        {
            eventTrigger = gameObject.AddComponent<EventTrigger>();
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

    private bool _firstEnable = true;
    public void OnEnable()
    {
        if (_firstEnable)
        {
            _firstEnable = false;
            return;
        }
        text.color = originalTextColor;
        panel.color = new Color(0, 0, 0, 0);
    }


    private void OnPointerEnter()
    {
        _sfx.PlayHover();
        text.color = hoverTextColor;
        StopAllCoroutines();
        StartCoroutine(FadePanel(true));
    }

    private void OnPointerExit()
    {
        text.color = originalTextColor;
        StopAllCoroutines();
        StartCoroutine(FadePanel(false));
    }

    private IEnumerator FadePanel(bool fadeIn)
    {
        float targetAlpha = fadeIn ? 1.0f : 0.0f;
        float currentAlpha = panel.color.a;
        float fadeSpeed = 5.0f; 

        while (!Mathf.Approximately(currentAlpha, targetAlpha))
        {
            currentAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, fadeSpeed * Time.unscaledDeltaTime);
            Color newColor = panel.color;
            newColor.a = currentAlpha;
            panel.color = newColor;

            yield return null;
        }
        panel.color = new Color(0, 0, 0, targetAlpha);
    }
}