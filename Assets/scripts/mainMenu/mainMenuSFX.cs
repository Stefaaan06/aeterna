using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainMenuSFX : MonoBehaviour
{
    public AudioSource AudioSource;
    public AudioClip[] clips;
    
    public void PlayHover()
    {
        AudioSource.pitch = Random.Range(2f, 3f);
        AudioSource.PlayOneShot(clips[0], 1f);
    }
    
    public void PlayClick()
    {
        AudioSource.pitch = 2.5f;
        AudioSource.PlayOneShot(clips[1], 1f);
    }
    
}
