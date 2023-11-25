
using System;
using UnityEngine;
public class singelton : MonoBehaviour
{
    private static singelton instance;

    public static singelton Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<singelton>();

                if (instance == null)
                {
                    instance = new GameObject("SingletonManager").AddComponent<singelton>();
                }
            }

            return instance;
        }
    }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
