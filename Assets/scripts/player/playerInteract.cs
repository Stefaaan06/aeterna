using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerInteract : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private Transform cam;

    [SerializeField] private int range;

    private GameObject _target;
    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, range, targetLayer))
        {
            _target = hit.transform.gameObject;
            if (Input.GetKeyDown(KeyCode.E))
            {

                _target.GetComponent<playerButton>().buttonPress();
            }
            else
            {
                _target.GetComponent<playerButton>().buttonHighlight();
            }
        }
    }
}
