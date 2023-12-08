using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class button : MonoBehaviour
{
    public UnityEvent onEnterEvent; 
    public UnityEvent onExitEvent;

    private bool _enter;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 9 && !_enter || other.gameObject.layer == 11 && !_enter)
        {
            onEnterEvent.Invoke();
            _enter = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 9 || other.gameObject.layer == 11)
        {
            onExitEvent.Invoke();
            _enter = false;
        }
    }
}
