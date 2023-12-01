using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class cube : MonoBehaviour
{
    public int extraGravityStrength = 10;
    public int maxSpeed = 50;
    private Rigidbody _rb;
    
    AudioSource _source;  
    AudioClip[] clips;     
    float cooldownTime = 0.1f; 
    float lastActionTime; 
    private int startIndex;

    private void Awake()
    {
        startIndex = SceneManager.GetActiveScene().buildIndex;
        _rb = GetComponent<Rigidbody>();
    }
    
    void Start()
    {
        _source = this.GetComponent<AudioSource>();
        if (_source == null)
        {
            _source = this.AddComponent<AudioSource>();
        }
        
        _source.spatialBlend = 1f;
        _source.maxDistance = 60f;
        _source.rolloffMode = AudioRolloffMode.Custom;
        _source.clip = Resources.Load<AudioClip>("SFX/boxHit");
        _source.outputAudioMixerGroup = Resources.Load<AudioMixerGroup>("SFX");
    }

    private void OnEnable()
    {
        if (SceneManager.GetActiveScene().buildIndex != startIndex)
        {
            Destroy(this.gameObject);
        }
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
    
   

    void OnCollisionEnter(Collision collision){    
        if (IsActionAvailable())   
        {
            lastActionTime = Time.time;   
            float x = collision.relativeVelocity.magnitude;    
            if (collision.relativeVelocity.magnitude > 5)
            {
                _source.volume =  0.2f;
                _source.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
                _source.Play();
            }
        }
    }


    public bool IsActionAvailable()
    {
        return Time.time - lastActionTime >= cooldownTime;  
    }
}
