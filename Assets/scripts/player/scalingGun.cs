using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class scalingGun : MonoBehaviour
{
    public int range;
    public Camera cam;

    private scalableObject[] scalableObjects;
    public AudioSource src;
    private void Start()
    {
        findScalables();
    }

    void findScalables()
    {
        scalableObjects = FindObjectsOfType<scalableObject>();
    }

    private bool play;
    void Update()
    {
        if (Input.GetButton("Fire1") || Input.GetKey(KeyCode.X))
        { 
            foreach (scalableObject scalable in scalableObjects)
            {
                scalable.ScaleUp(25, false);
                if (!src.isPlaying)
                {
                    play = true;
                    src.pitch = 1.3f;
                    src.Play();
                }
            }
        }else if (Input.GetButton("Fire2") || Input.GetKey(KeyCode.C))
        {
            foreach (scalableObject scalable in scalableObjects)
            {
                scalable.ScaleDown(false);
                if (!src.isPlaying)
                {
                    play = true;
                    src.pitch = 0.7f;
                    src.Play();
                };
            }
        }else
        {
            if (play)
            {
                play = false;
                StartCoroutine(FadeOut());
            }
        }
    }
    
    IEnumerator FadeOut()
    {
        float startVolume = src.volume;

        while (src.volume > 0)
        {
            src.volume -= startVolume * Time.deltaTime / 0.2f;
            yield return null;
        }

        src.Pause();
        src.volume = 1;
    }
    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        findScalables();
        
    }
    
    
    /// <summary>
    /// obsolete? 
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
