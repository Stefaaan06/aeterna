using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;


/// <summary>
/// Holds the points of the designated area to repeat & draws a gizmo
/// <author>Stefaaan06</author>
/// <version>1.0.0</version>
/// </summary>
public class RepeatingArea : MonoBehaviour
{
    public Vector3 size = new Vector3(100,100,100);     //size of the area
    [HideInInspector] public Renderer[] allRenderers;
    
    private Material _material;
    private void Start()
    {
        BoxCollider col = this.AddComponent<BoxCollider>();
        col.isTrigger = true;
        col.size = size;
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("player") || other.CompareTag("repeat"))
        {
            if(Vector3.Distance(this.transform.position, other.transform.position) > size.magnitude * 2)
            {
                return;
            }
            
            Vector3 newSize = size / 2;
            Vector3 currentPosition = other.transform.position;
            
            if (currentPosition.y < transform.position.y - newSize.y)
            {
                other.transform.position = new Vector3(other.transform.position.x, other.transform.position.y + size.y, other.transform.position.z);
            }
            else if(currentPosition.x < transform.position.x - newSize.x)
            {
                other.transform.position = new Vector3(other.transform.position.x + size.x, other.transform.position.y, other.transform.position.z);
            }else if(currentPosition.x > transform.position.x + newSize.x)
            {
                other.transform.position = new Vector3(other.transform.position.x - size.x, other.transform.position.y, other.transform.position.z);
            }
            else if(currentPosition.z < transform.position.z - newSize.z)
            {
                other.transform.position = new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z + size.z);
            }else if (currentPosition.z > transform.position.z - newSize.z)
            {
                other.transform.position = new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z - size.z);
            }
        }
    }
    
    /// <summary>
    /// Handles the Gizmo drawing of the Area
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1,0,0,0.2f);
        Gizmos.DrawCube(this.transform.position, size);
        
        allRenderers = FindRenderersInArea();

        foreach (Renderer renderer in allRenderers)
        {
            if (IsRendererFullyContained(renderer))
            {
                Gizmos.color = new Color(0, 0, 1, 1f);
                Bounds bounds = renderer.bounds;
                Gizmos.DrawWireCube(bounds.center, bounds.size);
            }
        }
    }

    /// <summary>
    /// Handles the Gizmo drawing of the Area when selected
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawCube(this.transform.position, size);
    }
    
    /// <summary>
    /// Finds all Rendered objects in the Gizmo area
    /// </summary>
    /// <returns>returns all rendered Objects in the Gizmo</returns>
    private Renderer[] FindRenderersInArea()
    {
        allRenderers = FindObjectsOfType<Renderer>();

        List<Renderer> renderersInArea = new List<Renderer>();

        foreach (Renderer renderer in allRenderers)
        {
            if (IsRendererFullyContained(renderer))
            {
                renderersInArea.Add(renderer);
            }
        }

        return renderersInArea.ToArray();
    }

    /// <summary>
    /// Checks if the rendered Object is fully contained in the Gizmo area
    /// </summary>
    /// <param name="renderer">rendered Object to check</param>
    /// <returns>true or false depending on the result</returns>
    private bool IsRendererFullyContained(Renderer renderer)
    {
        Bounds gizmoBounds = new Bounds(transform.position, size);
        Bounds rendererBounds = renderer.bounds;

        // Check if the renderer is fully contained within the gizmo area
        return gizmoBounds.Contains(rendererBounds.min) && gizmoBounds.Contains(rendererBounds.max);
    }
}
