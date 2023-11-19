using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class callEventWithDelay : MonoBehaviour
{
    public UnityEvent unityEvent;
    public float delayTime;

    public void callEvent()
    {
        StartCoroutine(delay());
    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(delayTime);
        unityEvent.Invoke();
    }
}
