using UnityEngine.UI;
using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

//manages player vfx, sfx and other effects
public class playerFX : MonoBehaviour
{
    public Image panel;
    public AudioSource source;
    public AudioClip[] walkClips;
    public PlayerMovement playerMovement;
    public WallRunning wallRun;
    public Rigidbody player;
    public Transform grounded;

    public AudioSource fallSource;
    
    [SerializeField] private float wallRunDistance = 1f;

    
    [SerializeField] private float baseStepSpeed = 0.5f;

    private bool cameraShake;
    private void Start()
    {
        cameraShake = PlayerPrefs.GetInt("cameraShake") == 0;
        
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Level 1"))
        {
            fadeToNormalLong();
        }
        else
        {
            fadeToNormal();
        }
    }

    public void Update()
    {
        PlayerMovementSound();
        fastFall();
    }

    void fastFall()
    {
        float targetVolume = 0.2f; 

        if (player.linearVelocity.magnitude > 35 && !fallSource.isPlaying)
        {
            fallSource.Play();
        }
        else if (fallSource.isPlaying && player.linearVelocity.magnitude < 35)
        {
            fallSource.volume = Mathf.Lerp(fallSource.volume, 0.0f, Time.deltaTime * 4);
        
            if (fallSource.volume < 0.01f)
            {
                fallSource.Pause();
            }
        }
        else if (fallSource.isPlaying)
        {
            fallSource.volume = Mathf.Lerp(fallSource.volume, targetVolume, Time.deltaTime);
        }
    }

    public void Jump()
    {
        source.PlayOneShot(walkClips[UnityEngine.Random.Range(0, 10 - 1)], 0.7f);
        if (cameraShake)
        {
            CameraShaker.CameraShaker.Instance.ShakeOnce(1.2f, 1.2f, 0.2f, 0.5f);
        }
    }

    private float footstepsTimer = 0;
    void PlayerMovementSound(){
        if (playerMovement.grounded == true || wallRun.isWallRunning){
            if (playerMovement.moving){
                
                footstepsTimer -= Time.deltaTime;  

                if(footstepsTimer <= 0){    
                    source.pitch = UnityEngine.Random.Range(0.8f, 1.1f);
                    source.PlayOneShot(walkClips[UnityEngine.Random.Range(0, 10 - 1)], 0.3f);
                    footstepsTimer = baseStepSpeed;
                    if (cameraShake)
                    {
                        CameraShaker.CameraShaker.Instance.ShakeOnce(1f, 1, 0.2f, 1f);
                    }
                }
            }
        }
    }
    
    private Collider[] _hitColliders;
    private Collider[] _walkColliders;
    private float _velocity;

    public void playCollisionSound(Collision collision)
    {
        _velocity = collision.relativeVelocity.magnitude; 
        if (!wallRun.isWallRunning) 
        {
            if (playerMovement.grounded)
            {
                _hitColliders =
                    Physics.OverlapSphere(grounded.position, 2); 
            }
            else
            {
                if (_velocity > 15)
                {
                    _hitColliders =
                        Physics.OverlapSphere(grounded.position, 2); 
                }
            }
        }
        else
        {
            _hitColliders = Physics.OverlapSphere(player.position, wallRunDistance);
        }

        if (_hitColliders == null) return;
        foreach (var hitCollider in _hitColliders)
        {
            if (player.linearVelocity.magnitude > 35) 
            {
                source.pitch = UnityEngine.Random.Range(0.5f , 0.8f);
                if (cameraShake)
                {
                    CameraShaker.CameraShaker.Instance.ShakeOnce(_velocity / 18, _velocity / 15, 0.1f, 0.2f);
                }
            }
            else
            {
                source.pitch = UnityEngine.Random.Range(0.8f , 1.1f);
                if (cameraShake)
                {
                    CameraShaker.CameraShaker.Instance.ShakeOnce(1.2f, 1, 0.1f, 0.1f);
                }
            }
            source.PlayOneShot(walkClips[UnityEngine.Random.Range(0, 10 - 1)], _velocity / 35);
        }
    }

    public void shakeEarthquake()
    {

        CameraShaker.CameraShaker.Instance.Shake(CameraShaker.CameraShakePresets.Earthquake);
    }
    public void fadeToNormalLong()
    {
        StartCoroutine(FadePanel(20, 0f)); 

    }
    public void fadeToNormal()
    {
        StartCoroutine(FadePanel(6, 0f)); 
    }
    
    public void fadeToBlack()
    {
        StartCoroutine(FadePanel(4, 1f)); 
    }
    
    private IEnumerator FadePanel(float duration, float targetAlpha)
    {
        Color startColor = panel.color;
        Color targetColor = new Color(0, 0, 0, targetAlpha);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            panel.color = Color.Lerp(startColor, targetColor, elapsed / duration);

            yield return null;

            elapsed += Time.deltaTime;
        }

        panel.color = targetColor;
    }
}

