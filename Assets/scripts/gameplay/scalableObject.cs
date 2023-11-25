using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class scalableObject : MonoBehaviour
{
    
    private Vector3 _baseScale;
    public Vector3 maxScale;
    public Vector3 minScale;
    public Vector3 scaleSpeed;
    public float playerBoost = 1f;
    public bool reverse; // if true, scale down instead of up
    
    private bool _playerContact = false;
    private bool _otherContact = false;
    
    private Rigidbody _otherRb;
    private Rigidbody player;
    private PlayerMovement _playerMovement;
    private Collider _col;

    
    private void OnEnable()
    {
        EventManager.Instance.PickupEventGlobalEvent += HandleGlobalEvent;
    }

    private void OnDisable()
    {
        EventManager.Instance.PickupEventGlobalEvent -= HandleGlobalEvent;
    }
    
    private void HandleGlobalEvent()
    {
        _otherContact = false;
    }
    
    private void Start()
    {
        _playerMovement = FindObjectOfType<PlayerMovement>();
        player = FindObjectOfType<PlayerMovement>().GetComponent<Rigidbody>();
        _col = GetComponent<Collider>();
    }

    private readonly int _boostCooldown = 5;
    private int _boostCooldownTimer = 0;
    private bool _cooldown;
    public void FixedUpdate()
    {
        if (_boostCooldownTimer > 0)
        {
            _boostCooldownTimer--;
        }
        else
        {
            _cooldown = false;
            _playerMovement.canMove = true;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("player"))
        {
            _playerContact = true;
        }

        if (other.gameObject.CompareTag("repeat"))
        {
            _otherContact = true;
            _otherRb = other.gameObject.GetComponent<Rigidbody>();
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("player"))
        {
            _playerContact = false;
            //check if player is holding object while leaving collider 
            if(other.gameObject.GetComponentInChildren<extraGrav>())
            {
                _otherContact = false;
            }
        }
        if (other.gameObject.CompareTag("repeat"))
        {
            _otherContact = false;
        }
    }
    
    public void ScaleUp(float boostForce, bool stop)
    {
        if (reverse && !stop)
        {
            ScaleDown(true);
            return;
        }
        
        if (transform.localScale.x < maxScale.x || transform.localScale.y < maxScale.y || transform.localScale.z < maxScale.z)
        {
            _col.enabled = true;
            if (_playerContact && !_cooldown)
            {
                // Calculate the relative position vector between the player and the object with this script.
                Vector3 relativePosition = player.position - transform.position;

                // Apply the boost force only in directions where scaleSpeed is not 0.
                Vector3 boostForceVector = new Vector3(
                    scaleSpeed.x != 0 ? relativePosition.x : 0,
                    scaleSpeed.y != 0 ? relativePosition.y : 0,
                    scaleSpeed.z != 0 ? relativePosition.z : 0
                );

                player.AddForce(boostForceVector.normalized * (boostForce * playerBoost), ForceMode.Impulse);

                _cooldown = true;
                _playerMovement.canMove = false;
                _boostCooldownTimer = _boostCooldown;
            }else if (_otherContact)
            {
                // Calculate the relative position vector between the player and the object with this script.
                Vector3 relativePosition = _otherRb.position - transform.position;

                // Apply the boost force only in directions where scaleSpeed is not 0.
                Vector3 boostForceVector = new Vector3(
                    scaleSpeed.x != 0 ? relativePosition.x : 0,
                    scaleSpeed.y != 0 ? relativePosition.y : 0,
                    scaleSpeed.z != 0 ? relativePosition.z : 0
                );

                _otherRb.AddForce(boostForceVector.normalized * boostForce, ForceMode.Impulse);
            }
            
            Vector3 newScale = transform.localScale + scaleSpeed * Time.deltaTime;
            transform.localScale = new Vector3(
                Mathf.Min(newScale.x, maxScale.x),
                Mathf.Min(newScale.y, maxScale.y),
                Mathf.Min(newScale.z, maxScale.z)
            );
            foreach (Transform childTransform in transform)
            {
                childTransform.localScale = new Vector3(
                    Mathf.Min(newScale.x, maxScale.x),
                    Mathf.Min(newScale.y, maxScale.y),
                    Mathf.Min(newScale.z, maxScale.z)
                );
            }
        }
    }
    
    public void ScaleDown(bool stop)
    {
        if (reverse && !stop)
        {
            ScaleUp(25, true);
            return;
        }
        if (transform.localScale.x > minScale.x || transform.localScale.y > minScale.y || transform.localScale.z > minScale.z)
        {
            Vector3 newScale = transform.localScale - scaleSpeed * Time.deltaTime;
            transform.localScale = new Vector3(
                Mathf.Max(newScale.x, minScale.x),
                Mathf.Max(newScale.y, minScale.y),
                Mathf.Max(newScale.z, minScale.z)
            );
            foreach (Transform childTransform in transform)
            {
                childTransform.localScale = new Vector3(
                    Mathf.Min(newScale.x, maxScale.x),
                    Mathf.Min(newScale.y, maxScale.y),
                    Mathf.Min(newScale.z, maxScale.z)
                );
            }
            if (this.transform.localScale == new Vector3(0, 0, 0))
            {
                _col.enabled = false;
            }
        }
    }
}
