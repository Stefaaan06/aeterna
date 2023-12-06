using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

public class changeFps : MonoBehaviour
{ 
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SetFPS(10);
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            SetFPS(20);
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            SetFPS(30);
        }
        else if (Input.GetKeyDown(KeyCode.F4))
        {
            SetFPS(40);
        }
        else if (Input.GetKeyDown(KeyCode.F5))
        {
            SetFPS(50);
        }
        else if (Input.GetKeyDown(KeyCode.F6))
        {
            SetFPS(60);
        }
        else if (Input.GetKeyDown(KeyCode.F7))
        {
            SetFPS(70);
        }
        else if (Input.GetKeyDown(KeyCode.F8))
        {
            SetFPS(80);
        }
        else if (Input.GetKeyDown(KeyCode.F9))
        {
            SetFPS(100);
        }
        else if (Input.GetKeyDown(KeyCode.F10))
        {
            SetFPS(120);
        }
        else if (Input.GetKeyDown(KeyCode.F11))
        {
            SetFPS(140);
        }
        else if (Input.GetKeyDown(KeyCode.F12))
        {
            SetFPS(200);
        }
    }

    private void SetFPS(int targetFPS)
    {
        Application.targetFrameRate = targetFPS;

        Debug.Log("Target FPS set to: " + targetFPS);
    }
}

#endif
