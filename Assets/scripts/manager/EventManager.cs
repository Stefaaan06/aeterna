using Unity.VisualScripting;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private static EventManager instance;

    public static EventManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EventManager>();
            }
            return instance;
        }
    }

    public delegate void PickupEventGlobal();
    public event PickupEventGlobal PickupEventGlobalEvent;
    
    public void invokePickupEventGlobal()
    {
        PickupEventGlobalEvent?.Invoke();
    }
}