using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class setAnimatorTrigger : MonoBehaviour
{
    public string triggerName;

    private Animator _animator;
    public void Start()
    {
        if (triggerName == "")
        {
            Debug.LogError("No trigger name set!", this);
        }
        _animator = this.GetComponent<Animator>();
    }

    public void setTrigger()
    {
        _animator.SetTrigger(triggerName);
    }
}
