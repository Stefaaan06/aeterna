using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class limitFPS : MonoBehaviour
{
    public int targetFPS = 140;
    
        void Start()
        {
            Application.targetFrameRate = targetFPS;
        }
}
