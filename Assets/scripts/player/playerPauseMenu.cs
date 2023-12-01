using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerPauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject player;
    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public bool paused = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !paused)
        {
            Pause();
        }else if (Input.GetKeyDown(KeyCode.Escape) && paused)
        {
            unpause();
        }
    }


    void Pause()
    {
        paused = true;
        pauseMenu.SetActive(true);
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        
    }

    void unpause()
    {
        paused = false;
        pauseMenu.SetActive(false);
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    public void continueGame()
    {
        unpause();
    }

    public void retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        cube[] delete = FindObjectsOfType<cube>();
        foreach (cube g in delete)
        {
            Destroy(g.gameObject);
        }
        Destroy(player);
    }
    
    public void Quit()
    {
        Application.Quit(); 
    }

    
    public void backToMenu()
    {
        SceneManager.LoadScene(0);
        Destroy(player);
    }
}
