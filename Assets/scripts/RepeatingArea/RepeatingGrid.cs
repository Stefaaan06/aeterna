
#if (UNITY_EDITOR) 

using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Handles the duplication of the Rendered objects found in the RepeatingArea.cs script
/// <author>Stefaaan06</author>
/// <version>1.0.0</version>
/// </summary>
[Serializable]
public class RepeatingGrid : MonoBehaviour
{ 
    [SerializeField] private Vector3 repeatingTimes = new Vector3(1, 1, 1);
    [SerializeField] private Vector3 fullRepeatingTimes = new Vector3(1, 1, 1);
    [SerializeField] private RepeatingArea repeatingArea;
    
    /// <summary>
    /// Duplicates the Renders found in the RepeatingArea.cs script
    /// </summary>
    
    public void DuplicateRenderers()
    {
        Transform repeatingAreaTransform = repeatingArea.transform;
        Vector3 repeatingAreaPosition = repeatingAreaTransform.position;

        GameObject duplicateObjectParent = new GameObject("duplicateObjectParent");
        
        
        // Iterate through the grid cells
        for (int x = -(int)repeatingTimes.x; x <= (int)repeatingTimes.x; x++)
        {
            GameObject duplicateObjectColumn = new GameObject("duplicateObject: " + x);
            duplicateObjectColumn.transform.parent = duplicateObjectParent.transform;
            
            for (int y = -(int)repeatingTimes.y; y <= (int)repeatingTimes.y; y++)
            {
                GameObject duplicateObjectRow = new GameObject("duplicateObject: " + x + ";" + y);
                duplicateObjectRow.transform.parent = duplicateObjectColumn.transform;
                
                for (int z = -(int)repeatingTimes.z; z <= (int)repeatingTimes.z; z++)
                {
                    // Skip the middle cell
                    if (x == 0 && y == 0 && z == 0)
                        continue;

                    CopyRenderers(repeatingArea, duplicateObjectRow, repeatingAreaPosition, x, y, z);
                }
            }
        }
    }
    private void CopyRenderers(RepeatingArea sourceArea, GameObject duplicateObjectRow, Vector3 repeatingAreaPosition, int x, int y, int z)
    {
    foreach (Renderer renderer in sourceArea.allRenderers)
    {
        if (renderer.gameObject.CompareTag("ignoreDuplicate"))continue;

        if (!renderer.gameObject.CompareTag("repeat"))
        {
           if (Mathf.Abs(x) <= fullRepeatingTimes.x && Mathf.Abs(y) <= fullRepeatingTimes.y && Mathf.Abs(z) <= fullRepeatingTimes.z && (x != 0 || y != 0 || z != 0))
           {
               CopyEverything(renderer.gameObject, repeatingAreaPosition, x, y, z, duplicateObjectRow);
               continue;
           } 
        }
        
        GameObject duplicateObject = new GameObject("duplicateObject: " + x + ";" + y + ";" + z);
        duplicateObject.transform.parent = duplicateObjectRow.transform;
        
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

            // Set location, rotation & scale
            newRenderer.transform.localRotation = renderer.transform.localRotation;
            newRenderer.transform.localScale = renderer.transform.localScale;

            // Calculations to find the right position to place the new renderer
            Vector3 relativePosition = renderer.transform.position - repeatingAreaPosition;
            Vector3 gridCenter = new Vector3(
                repeatingAreaPosition.x + repeatingArea.size.x * x,
                repeatingAreaPosition.y + repeatingArea.size.y * y,
                repeatingAreaPosition.z + repeatingArea.size.z * z
            );

            newRenderer.transform.position = gridCenter + relativePosition;

            if (renderer.gameObject.CompareTag("repeat"))
            {
                newObj.gameObject.AddComponent<repeat>();
                newObj.gameObject.GetComponent<repeat>().setValues(renderer.gameObject, x, y,z, repeatingArea.size);
            }
            else
            {
                newObj.isStatic = true;
            }
        }
    }
}
    
private void CopyEverything(GameObject originalObject, Vector3 repeatingAreaPosition, int x, int y, int z, GameObject duplicateObjectRow)
{
    GameObject newGameObject = Instantiate(originalObject);
    newGameObject.name = "duplicateObject: " + x + ";" + y + ";" + z;
    newGameObject.transform.parent = duplicateObjectRow.transform;
    
    Vector3 relativePosition = originalObject.transform.position - repeatingAreaPosition;
    Vector3 gridCenter = new Vector3(
        repeatingAreaPosition.x + repeatingArea.size.x * x,
        repeatingAreaPosition.y + repeatingArea.size.y * y,
        repeatingAreaPosition.z + repeatingArea.size.z * z
    );

    newGameObject.transform.position = gridCenter + relativePosition;
}
    
    
    
    
    
    
    
    
    private void OnDrawGizmosSelected()
    {
        drawGrid(false);
    }

    /// <summary>
    /// Handles the Gizmo drawing 
    /// </summary>
    /// <param name="highlited">determines if the Gizmo should be drawn highlited or not</param>
    void drawGrid(bool highlited)
    {
        if (repeatingTimes.x > 15 || repeatingTimes.y > 15 || repeatingTimes.z > 15)
        {
            Debug.LogError("RepeatingTimes is higher than 15. This will cause significant performance issues. Resetting now.");
            repeatingTimes = new Vector3(1,1,1);
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
        for (int x = -(int)repeatingTimes.x; x <= (int)repeatingTimes.x; x++)
        {
            for (int y = -(int)repeatingTimes.y; y <= (int)repeatingTimes.y; y++)
            {
                for (int z = -(int)repeatingTimes.z; z <= (int)repeatingTimes.z; z++)
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
    private SerializedProperty _repeatingTimes;
    private SerializedProperty _fullRepeatingTimes;
    private SerializedProperty _repeatingArea;
    
    private GUIStyle _tooltipStyle;

    private void OnEnable()
    {
        // Create a GUIStyle for tooltips
        _tooltipStyle = new GUIStyle();
        _tooltipStyle.normal.textColor = Color.white;
        _tooltipStyle.wordWrap = true;

        _repeatingTimes = serializedObject.FindProperty("repeatingTimes");
        _repeatingArea = serializedObject.FindProperty("repeatingArea");
        _fullRepeatingTimes = serializedObject.FindProperty("fullRepeatingTimes");
    }

    /// <summary>
    /// Updates GUI elements
    /// </summary>
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GUILayout.Label("RepeatingGridEditor", EditorStyles.boldLabel);

        _repeatingTimes.vector3Value = EditorGUILayout.Vector3Field(new GUIContent("Repeating Times", "Amount of times to repeat"), _repeatingTimes.vector3Value);
        _fullRepeatingTimes.vector3Value = EditorGUILayout.Vector3Field(new GUIContent("Full Repeating Times", "The area in which all components should be copied as well (may be performance heavy)"), _fullRepeatingTimes.vector3Value);
        _repeatingArea.objectReferenceValue = EditorGUILayout.ObjectField(new GUIContent("Repeating Area", "The RepeatingArea script to use"), _repeatingArea.objectReferenceValue, typeof(RepeatingArea), true) as RepeatingArea;

        
        if (GUILayout.Button("Duplicate Renderers"))
        {
            if (_repeatingTimes.vector3Value.x > 5 || _repeatingTimes.vector3Value.y > 5 || _repeatingTimes.vector3Value.z > 5) // Check if repeatingTimes is higher than 5
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

#endif

