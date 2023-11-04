
using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

/// <summary>
/// Handles the duplication of the Rendered objects found in the RepeatingArea.cs script
/// <author>Stefaaan06</author>
/// <version>1.0.0</version>
/// </summary>
[Serializable]
public class RepeatingGrid : MonoBehaviour
{ 
    [SerializeField] private int repeatingTimes = 1;
    [SerializeField]private RepeatingArea repeatingArea;
    
    /// <summary>
    /// Duplicates the Renders found in the RepeatingArea.cs script
    /// </summary>
    
    public void DuplicateRenderers()
    {
        Transform repeatingAreaTransform = repeatingArea.transform;
        Vector3 repeatingAreaPosition = repeatingAreaTransform.position;

        GameObject duplicateObjectParent = new GameObject("duplicateObjectParent");
        
        // Iterate through the grid cells
        for (int x = -repeatingTimes - 1; x < repeatingTimes; x++)
        {
            GameObject duplicateObjectColumn = new GameObject("duplicateObject: " + x);
            duplicateObjectColumn.transform.parent = duplicateObjectParent.transform;
            
            for (int y = -repeatingTimes - 1; y < repeatingTimes; y++)
            {
                GameObject duplicateObjectRow = new GameObject("duplicateObject: " + x + ";" + y);
                duplicateObjectRow.transform.parent = duplicateObjectColumn.transform;
                
                for (int z = -repeatingTimes - 1; z < repeatingTimes; z++)
                {
                    GameObject duplicateObject = new GameObject("duplicateObject: " + x + ";" + y + ";" + z);
                    duplicateObject.transform.parent = duplicateObjectRow.transform;
                    
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
                            GameObject newObj = new GameObject("newObject");
                            newObj.transform.parent = duplicateObject.transform;
                            
                            MeshFilter newMeshFilter = newObj.AddComponent<MeshFilter>();
                            MeshRenderer newRenderer = newObj.AddComponent<MeshRenderer>();

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

    private void OnDrawGizmos()
    {
        drawGrid(false);
    }

    /// <summary>
    /// Handles the Gizmo drawing 
    /// </summary>
    /// <param name="highlited">determines if the Gizmo should be drawn highlited or not</param>
    void drawGrid(bool highlited)
    {
        if (repeatingTimes > 10)
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
    
    
}
[CustomEditor(typeof(RepeatingGrid))]
public class RepeatingGridEditor : Editor
{
    SerializedProperty repeatingTimes;
    SerializedProperty repeatingArea;

    private GUIStyle tooltipStyle;

    private void OnEnable()
    {
        // Create a GUIStyle for tooltips
        tooltipStyle = new GUIStyle();
        tooltipStyle.normal.textColor = Color.white;
        tooltipStyle.wordWrap = true;

        repeatingTimes = serializedObject.FindProperty("repeatingTimes");
        repeatingArea = serializedObject.FindProperty("repeatingArea");
    }

    /// <summary>
    /// Updates GUI elements
    /// </summary>
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GUILayout.Label("RepeatingGridEditor", EditorStyles.boldLabel);

        repeatingTimes.intValue = EditorGUILayout.IntField(new GUIContent("Repeating Times", "Amount of times to repeat"), repeatingTimes.intValue);
        repeatingArea.objectReferenceValue = EditorGUILayout.ObjectField(new GUIContent("Repeating Area", "The RepeatingArea script to use"), repeatingArea.objectReferenceValue, typeof(RepeatingArea), true) as RepeatingArea;

        if (GUILayout.Button("Duplicate Renderers"))
        {
            if (repeatingTimes.intValue > 5) // Check if repeatingTimes is higher than 5
            {
                if (EditorUtility.DisplayDialog("Warning", "RepeatingTimes is higher than 5. This will cause significant performance issues. Are you sure you want to continue?", "Yes", "No"))
                {
                    (target as RepeatingGrid).DuplicateRenderers();
                }
            }
            else
            {
                (target as RepeatingGrid).DuplicateRenderers();
            }
        }
        serializedObject.ApplyModifiedProperties();
    }
}


