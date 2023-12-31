using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class playerTrigger : MonoBehaviour
{
    public UnityEvent onTriggerEnter;
    public UnityEvent onTriggerExit;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(Time.deltaTime);   
        if (other.CompareTag("player"))
        {
            Debug.Log("x");
            onTriggerEnter.Invoke();
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("player"))
        {
            onTriggerExit.Invoke();
        }
    }
}
