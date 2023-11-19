using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class playerButton : MonoBehaviour
{
    public UnityEvent onButtonPress;
    public UnityEvent onButtonPressInstant;
    public UnityEvent onButtonPressInstantNoDisable;
    public UnityEvent onButtonHighlight;

    public bool onePress;
    public bool disableThis;
    public bool delayButtonPress;
    public bool delayButtonHighlight;
    public float delayTime;
    
    private bool _pressed = false;
    public void buttonHighlight()
    {
        
        onButtonHighlight.Invoke();
    }
    
    public void buttonPress()
    {
        
        onButtonPressInstantNoDisable.Invoke();
        
        if(onePress && _pressed) return;
        
        onButtonPressInstant.Invoke();
        
        if (delayButtonPress)
        {
            StartCoroutine(delayButton());
            return;
        }
        
        if(disableThis) gameObject.SetActive(false);
        onButtonPress.Invoke();
        _pressed = true;
    }

    IEnumerator delayButton()
    {
        yield return new WaitForSeconds(delayTime);

        if (!onePress || !_pressed)
        {
            if(disableThis) gameObject.SetActive(false);
            onButtonPress.Invoke();
            _pressed = true;
        }
    }
}
