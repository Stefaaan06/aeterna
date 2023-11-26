using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portalTextureSetup : MonoBehaviour
{
    public Camera cameraA;
    public Camera cameraB;

    public GameObject renderA;
    public GameObject renderB;
    
    private Material _cameraMatA;
    private Material _cameraMatB;

    void Start () {
        _cameraMatA = new Material(Shader.Find("Unlit/ScreenCutoutShader"));
        _cameraMatB = new Material(Shader.Find("Unlit/ScreenCutoutShader"));
        
        renderA.transform.GetComponent<MeshRenderer>().material = _cameraMatB;
        renderB.transform.GetComponent<MeshRenderer>().material = _cameraMatA;

        if (cameraA.targetTexture != null)
        {
            cameraA.targetTexture.Release();
        }
        
        cameraA.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        _cameraMatA.mainTexture = cameraA.targetTexture;

        if (cameraB.targetTexture != null)
        {
            cameraB.targetTexture.Release();
        }
        
        cameraB.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        _cameraMatB.mainTexture = cameraB.targetTexture;
    }
}
