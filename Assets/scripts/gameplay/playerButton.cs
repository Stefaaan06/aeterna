using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class playerButton : MonoBehaviour
{
    public UnityEvent onButtonPress;
    public UnityEvent onButtonHighlight;

    public bool onePress;
    
    private bool _pressed = false;
    public void buttonHighlight()
    {
        
        onButtonHighlight.Invoke();
    }
    
    public void buttonPress()
    {
        if(onePress && _pressed) return;
        onButtonPress.Invoke();
        _pressed = true;

    }
}
