using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scalableObject : MonoBehaviour
{
    
    private Vector3 _baseScale;
    public Vector3 maxScale;
    public Vector3 minScale;
    public Vector3 scaleSpeed;
    public bool reverse; // if true, scale down instead of up

    private bool _playerContact = false;
    private bool _otherContact = false;
    
    private Rigidbody _otherRb;
    private Rigidbody player;
    private PlayerMovement _playerMovement;

    private void Start()
    {
        _playerMovement = FindObjectOfType<PlayerMovement>();
        player = FindObjectOfType<PlayerMovement>().GetComponent<Rigidbody>();
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
        
        if (transform.localScale.x < maxScale.x || transform.localScale.y < maxScale.y || transform.localScale.z < maxScale.z) {
            if (_playerContact && !_cooldown)
            {

                // Calculate the relative position vector between the player and the object with this script.
                Vector3 relativePosition = player.position - transform.position;

                // Apply the boost force in the direction of the relative position.
                player.AddForce(relativePosition.normalized * boostForce, ForceMode.Impulse);
            
                _cooldown = true;
                _playerMovement.canMove = false;
                _boostCooldownTimer = _boostCooldown;
            }else if (_otherContact)
            {
                // Calculate the relative position vector between the player and the object with this script.
                Vector3 relativePosition = _otherRb.position - transform.position;

                // Apply the boost force in the direction of the relative position.
                _otherRb.AddForce(relativePosition.normalized * boostForce, ForceMode.Impulse);
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
        }
    }
}
