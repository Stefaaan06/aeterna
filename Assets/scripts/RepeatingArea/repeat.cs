using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;

public class repeat : MonoBehaviour
{
    [SerializeField] private Transform _originalGameObj;
    [SerializeField] private int x;
    [SerializeField] private int y;
    [SerializeField] private int z;
    [SerializeField] private Vector3 _size;

    private runPosUpdate _runPosUpdate;
    private void Start()
    {
        _runPosUpdate = new runPosUpdate()
        {
            thisTransform = transform,
            transform = _originalGameObj,
            x = x,
            y = y,
            z = z,
            size = _size
        };
    }

    public void setValues(GameObject originalGameObject, int x, int y, int z, Vector3 size)
    {
        _originalGameObj = originalGameObject.transform;
        this.x = x;
        this.y = y;
        this.z = z;
        _size = size;
    }
    
    private void Update()
    {
        _runPosUpdate.Execute();
    }
}

public struct runPosUpdate : IJob
{
    public Transform thisTransform;
    public Transform transform;
    public int x;
    public int y;
    public int z;
    public Vector3 size;
    
    public void Execute()
    {
        thisTransform.position = new Vector3(transform.position.x + (x * size.x), transform.position.y + (y * size.y), transform.position.z + (z * size.z));
        thisTransform.rotation = transform.rotation;
    }
}
