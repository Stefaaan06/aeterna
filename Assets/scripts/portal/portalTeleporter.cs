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
    public Collider thisCol;
    
    private Transform _playerTransform;
    private bool _playerIsOverlapping = false;

    private int _cooldown = 50;
    private int _currentCooldown = 0;
    private bool _teleported;
    private PlayerMovement _playerMovement;

    void Start()
    {
        _playerMovement = FindObjectOfType<PlayerMovement>();
        _playerTransform = _playerMovement.transform;
    }

    private void FixedUpdate()
    {
        //cooldown
        if(_teleported)
        {
            _currentCooldown++;
            if (_currentCooldown >= _cooldown)
            {
                _teleported = false;
                _currentCooldown = 0;
                otherCol.enabled = true;
                thisCol.enabled = true;
            }
        }
    }

    void Update () {
        if (_playerIsOverlapping && !_teleported && _playerMovement.moving)
        {
            backCol.enabled = false;
            Vector3 portalToPlayer = _playerTransform.position - transform.position;
            float dotProduct = Vector3.Dot(transform.forward, portalToPlayer);


                float rotationDiff = -Quaternion.Angle(transform.rotation, recieverTransform.rotation);
                _playerTransform.Rotate(Vector3.up, rotationDiff);
                
                
                Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;
                _playerTransform.position = recieverTransform.position + positionOffset;

                _playerIsOverlapping = false;
                _teleported = true;
                otherCol.enabled = false;
                thisCol.enabled = false;
            
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
