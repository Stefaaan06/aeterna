using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class callEventOnStart : MonoBehaviour
{
    public UnityEvent unityEvent;

    private void Start()
    {
        unityEvent.Invoke();
    }
}
