using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class scalingGun : MonoBehaviour
{
    public int range;
    public Camera cam;

    private scalableObject[] scalableObjects;

    private void Start()
    {
        findScalables();
    }

    void findScalables()
    {
        scalableObjects = FindObjectsOfType<scalableObject>();
    }
    
    void Update()
    {
        if (Input.GetButton("Fire1"))
        { 
            foreach (scalableObject scalable in scalableObjects)
            {
                scalable.ScaleUp(25, false);
            }
        }else if (Input.GetButton("Fire2"))
        {
            foreach (scalableObject scalable in scalableObjects)
            {
                scalable.ScaleDown(false);
            }
        }
    }
    
    
    
    /// <summary>
    /// obsolote? 
    /// </summary>
    private RaycastHit _prevHit;
    private bool sameHit;
    private scalableObject scalableObject;
    void shoot(bool scaleUp)
    {
        bool raycastHit = Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, range);
        if(!raycastHit) return;
        if (hit.Equals(_prevHit))
        {
            sameHit = true;
        }else{
            _prevHit = hit;
            sameHit = false;
        }
        if (!sameHit)
        {
            hit.transform.gameObject.TryGetComponent<scalableObject>(out scalableObject);
            if (scalableObject == null) return;
        }
        if (!hit.transform.gameObject.CompareTag("scaleable")) return;

        if (scaleUp)
        {
            scalableObject.ScaleUp(100, false);
        }
        else
        {
            scalableObject.ScaleDown(false);
        }
    }
}
