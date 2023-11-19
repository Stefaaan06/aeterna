using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerParams : MonoBehaviour
{
    public float fogStrength;

    void Start()
    {
        FindObjectOfType<Fog>().fogDensity = fogStrength;
    }
}

