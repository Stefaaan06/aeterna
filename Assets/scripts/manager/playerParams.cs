using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class playerParams : MonoBehaviour
{
    public float fogStrength;
    public GameObject mainPlayer;
    public int farClipPlane = 7000;
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

        foreach (Fog fog in FindObjectsByType<Fog>(FindObjectsSortMode.None).ToArray())
        {
            fog.fogDensity = fogStrength;
        }

        foreach (Camera cam in FindObjectsByType<Camera>(FindObjectsSortMode.None))
        {
            cam.farClipPlane = farClipPlane;
        }
    }
}

 