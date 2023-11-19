using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portalTeleporter : MonoBehaviour
{
    public Transform player;
    public Transform reciever;

    private bool playerIsOverlapping = false;

    void LateUpdate () {
        if (playerIsOverlapping)
        {
            Debug.Log("overlapping");
            Vector3 portalToPlayer = player.position - transform.position;
            float dotProduct = Vector3.Dot(transform.up, portalToPlayer);

            if (dotProduct < 0f)
            {
                float rotationDiff = -Quaternion.Angle(transform.rotation, reciever.rotation);
                player.Rotate(Vector3.up, rotationDiff);

                Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;
                player.position = reciever.position + positionOffset;

                playerIsOverlapping = false;
            }
        }
    }

    void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag("player"))
        {
            playerIsOverlapping = true;
        }
    }
    
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("player"))
        {
            playerIsOverlapping = true;
        }
    }

    void OnTriggerExit (Collider other)
    {
        if (other.CompareTag("player"))
        {
            playerIsOverlapping = false;
        }
    }
}
