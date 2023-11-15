using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fadeChildren : MonoBehaviour
{

    public void fadeIn()
    {
        thisObjFadeController controller;
        bool success = false;
        
        foreach (Transform child in transform)
        {
            
            success = child.TryGetComponent<thisObjFadeController>(out controller);
            if(!success) return;
            controller.fadeIn();
        }
    }

    public void fadeOut()
    {
        thisObjFadeController controller;
        bool success = false;
        
        foreach (Transform child in transform)
        {
            success = child.TryGetComponent<thisObjFadeController>(out controller);
            if(!success) return;
            controller.fadeOut();
        }
    }
}
