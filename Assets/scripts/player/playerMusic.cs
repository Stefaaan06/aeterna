using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMusic : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioClip[] Music;
    public bool playAtStart = true;

    public void Start()
    {
        if(!playAtStart) return;
        Invoke("startMusicLoop", 3f);
    }
    
    public void playSpecificMusic(int num)
    {
        StopAllCoroutines();
        musicSource.PlayOneShot(Music[num], 0.6f);
    }

    public void stopMusicLoop()
    {
        StopAllCoroutines();
    }

    public void startMusicLoop()
    {
        StartCoroutine(musicLoop());
    }

    private int _num;
    IEnumerator musicLoop()
    {
        _num = UnityEngine.Random.Range(0, Music.Length - 1);   
        
        musicSource.PlayOneShot(Music[_num], 0.6f);
        yield return new WaitForSeconds(Music[_num].length);
        Music[_num].UnloadAudioData();

        int wait = UnityEngine.Random.Range(10, 20);
        yield return new WaitForSeconds(wait);

        //restarts Method
        StartCoroutine(musicLoop());
    }
    
    IEnumerator FadeOutMusic(float fadeOutDuration)
    {
        float startVolume = musicSource.volume;

        while (musicSource.volume > 0)
        {
            musicSource.volume -= startVolume * Time.deltaTime / fadeOutDuration;
            yield return null;
        }

        musicSource.volume = 0;
    }
}
