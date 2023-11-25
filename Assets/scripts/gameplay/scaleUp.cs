using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleUp : MonoBehaviour
{
    public float maxScale = 2.0f;  
    public float scaleSpeed = 0.5f;  

    private bool isScaling = false;


    void Update()
    {
        if (isScaling)
        {
            ScaleObject();
        }
    }

    public void InitializeScaling()
    {
        isScaling = true;
    }
    
    void ScaleObject()
    {
        if (transform.localScale.x < maxScale)
        {
            float newScale = Mathf.Min(transform.localScale.x + scaleSpeed * Time.deltaTime, maxScale);

            transform.localScale = new Vector3(newScale, newScale, newScale);
        }
        else
        {
            isScaling = false;
        }
    }
}