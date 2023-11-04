
using System;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Handles the duplication of the Rendered objects found in the RepeatingArea.cs script
/// <author>Stefaaan06</author>
/// <version>1.0.0</version>
/// </summary>
public class RepeatingGrid : MonoBehaviour
{
    public RepeatingArea repeatingArea;
    public int repeatingTimes = 5;
    
    /// <summary>
    /// Gets called on script load - duplicates the Renderers & disables the original ones
    /// </summary>
    private void Awake()
    {
        if (repeatingTimes > 6)
        {
            Debug.LogError("RepeatingTimes is higher than 10. This will cause significant performance issues.");
            repeatingTimes = 1;
        }
        DuplicateRenderers();
    }


    /// <summary>
    /// Duplicates the Renders found in the RepeatingArea.cs script
    /// </summary>
    void DuplicateRenderers()
    {
        Transform repeatingAreaTransform = repeatingArea.transform;
        Vector3 repeatingAreaPosition = repeatingAreaTransform.position;
        
        // Iterate through the grid cells
        for (int x = -repeatingTimes - 1; x < repeatingTimes; x++)
        {
            for (int y = -repeatingTimes - 1; y < repeatingTimes; y++)
            {
                for (int z = -repeatingTimes - 1; z < repeatingTimes; z++)
                {
                    // Skip the middle cell
                    if (x == -1 && y == -1 && z == -1)
                        continue;
                    
                    Vector3 drawPos = new Vector3(
                        repeatingAreaPosition.x + repeatingArea.size.x * x,
                        repeatingAreaPosition.y + repeatingArea.size.y * y,
                        repeatingAreaPosition.z + repeatingArea.size.z * z
                    );

                    // Iterate through renderers and duplicate them for the cube
                    foreach (Renderer renderer in repeatingArea.allRenderers)
                    {
                        MeshFilter meshFilter = renderer.GetComponent<MeshFilter>();

                        if (meshFilter != null)
                        {
                            GameObject cube = new GameObject("newObject");
                            
                            MeshFilter newMeshFilter = cube.AddComponent<MeshFilter>();
                            MeshRenderer newRenderer = cube.AddComponent<MeshRenderer>();
    
                            newMeshFilter.sharedMesh = meshFilter.sharedMesh;
                            newRenderer.sharedMaterials = renderer.sharedMaterials;


                            newRenderer.shadowCastingMode = ShadowCastingMode.Off;
                            newRenderer.receiveShadows = false;
                            
                            //set location, rotation & scale
                            newRenderer.transform.localRotation = renderer.transform.localRotation;
                            newRenderer.transform.localScale = renderer.transform.localScale;
                            
                            //calculations to find the right position to place the new renderer
                            Vector3 relativePosition = renderer.transform.position - repeatingAreaPosition;
                            Vector3 gridCenter = drawPos + repeatingArea.size;
                            newRenderer.transform.position = gridCenter + relativePosition;
                        }
                    }
                }
            }
        }
    }
    

    /// <summary>
    /// Handles the Gizmo drawing 
    /// </summary>
    /// <param name="highlited">determines if the Gizmo should be drawn highlited or not</param>
    void drawGrid(bool highlited)
    {
        if (repeatingTimes > 6)
        {
            Debug.LogError("RepeatingTimes is higher than 10. This will cause significant performance issues.");
            repeatingTimes = 1;
        }
        if (highlited)
        {
            Gizmos.color = new Color(1, 1, 0, 0.5f);
        }
        else
        {
            Gizmos.color = new Color(1, 1, 0, 0.1f);
        }
        
        Transform repeatingAreaTransform = repeatingArea.transform;
        Vector3 repeatingAreaPosition = repeatingAreaTransform.position;
        
        //draws the grid of cubes
        for (int x = -repeatingTimes; x <= repeatingTimes; x++)
        {
            for (int y = -repeatingTimes; y <= repeatingTimes; y++)
            {
                for (int z = -repeatingTimes; z <= repeatingTimes; z++)
                {
                    // Skip the middle cell
                    if (x == 0 && y == 0 && z == 0)
                        continue;
                        
                    
                    Vector3 drawPos = new Vector3(
                        repeatingAreaPosition.x + repeatingArea.size.x * x,
                        repeatingAreaPosition.y + repeatingArea.size.y * y,
                        repeatingAreaPosition.z + repeatingArea.size.z * z
                    );

                    Gizmos.DrawWireCube(drawPos, repeatingArea.size);

                    if (highlited)
                    {
                        Gizmos.color = new Color(1, 0, 0, 0.01f);
                        Gizmos.DrawCube(drawPos, repeatingArea.size);
                    }
                }
            }
        }
    }
    
    /// <summary>
    /// Gets called when the Gizmo gets drawn
    /// </summary>
    private void OnDrawGizmos()
    {
        drawGrid(false);
    }
    
    /// <summary>
    /// Gets called when the Gizmo gets selected
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        drawGrid(true);
    }
}
