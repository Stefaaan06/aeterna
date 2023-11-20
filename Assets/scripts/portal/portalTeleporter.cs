using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class portalTeleporter : MonoBehaviour
{
    public Transform recieverTransform;
    public Collider backCol;
    public Collider otherCol;
    
    private Transform _playerTransform;
    private Rigidbody _playerRigidbody;
    private bool _playerIsOverlapping = false;

    private int _cooldown = 160;
    private int _currentCooldown = 0;
    private bool _teleported;

    void Start()
    {
        _playerTransform = FindObjectOfType<PlayerMovement>().transform;
        _playerRigidbody = FindObjectOfType<PlayerMovement>().GetComponent<Rigidbody>();
    }

    void Update () {
        //cooldown
        if(_teleported)
        {
            _currentCooldown++;
            if (_currentCooldown >= _cooldown)
            {
                _teleported = false;
                _currentCooldown = 0;
                otherCol.enabled = true;
            }
        }
        if (_playerIsOverlapping && !_teleported)
        {
            backCol.enabled = false;
            Vector3 portalToPlayer = _playerTransform.position - transform.position;
            float dotProduct = Vector3.Dot(transform.forward, portalToPlayer);

            if (dotProduct < 0f)
            {
                float rotationDiff = -Quaternion.Angle(transform.rotation, recieverTransform.rotation);
                _playerTransform.Rotate(Vector3.up, rotationDiff);
                
                
                Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;
                _playerTransform.position = recieverTransform.position + positionOffset;

                _playerIsOverlapping = false;
                _teleported = true;
                otherCol.enabled = false;
                _playerRigidbody.AddForce(0, 0, -transform.forward.z * 1000);
            }
        }
        else
        {
            backCol.enabled = true;
        }
    }

    void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag("player"))
        {
            _playerIsOverlapping = true;
        }
    }
    
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("player"))
        {
            _playerIsOverlapping = true;
        }
    }

    void OnTriggerExit (Collider other)
    {
        if (other.CompareTag("player"))
        {
            _playerIsOverlapping = false;
        }
    }
}
