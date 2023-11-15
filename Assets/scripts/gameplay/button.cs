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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            onEnterEvent.Invoke();
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            onExitEvent.Invoke();
        }
    }
}
