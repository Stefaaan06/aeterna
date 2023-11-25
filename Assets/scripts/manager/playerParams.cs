using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerParams : MonoBehaviour
{
    public float fogStrength;
    public GameObject mainPlayer;
    void Start()
    {
        try
        {
            GameObject player = FindObjectOfType<PlayerMovement>().gameObject;
        }
        catch (NullReferenceException)
        {
            Debug.Log("no Player. Initializing mainPlayer");
            mainPlayer.SetActive(true);
        }
        
        FindObjectOfType<Fog>().fogDensity = fogStrength;
    }
}

 