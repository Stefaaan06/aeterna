using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class extraGrav : MonoBehaviour
{
    public int extraGravityStrength = 10;
    public int maxSpeed = 50;
    private Rigidbody _rb; 
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (maxSpeed < _rb.velocity.magnitude)
        {
            _rb.velocity = _rb.velocity.normalized * maxSpeed;
        }
        else
        {
            _rb.AddForce(Vector3.down * (Time.deltaTime * extraGravityStrength));
        }
       
    }
}
