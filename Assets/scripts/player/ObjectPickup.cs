using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/**
 * Description: Script allowing the Player to pick up Objects
 * @version 1.0.2 - player can rotate objects with mouse scroll
 * @author Stefaaan
 */

public class ObjectPickup : MonoBehaviour {
    //declaring Variables and References
    [Header("Pickup Settings")]
    [SerializeField] private Transform holdArea;
    [SerializeField] private Transform cam;
    [SerializeField] private LayerMask pickupLayer;

    [Header("References")]
    [SerializeField] private PlayerMovement playerMovement;

    private static GameObject _heldObj = null;
    private Rigidbody _heldObjRb;
    private Vector3 _scale;

   
    [Header("Physics Parameters")]
    [SerializeField] private float pickupRange;
    [SerializeField] private float pickupForce;
    

    float _objMass;
    private void Update(){
        if(Input.GetKeyDown(KeyCode.E)){
            if(_heldObj == null){    
                RaycastHit hit;
                //shoots Raycast to find Object within the PickupRange and having the correct layer
                if(Physics.Raycast(cam.position, cam.forward, out hit, pickupRange, pickupLayer)){
                    PickupObject(hit.transform.gameObject); 
                }
            }else{
                DropObject();
            }
        }
        if(_heldObj == null) return;
        rotateObject();
        MoveObject();
    }
    
    

    float _prevAngularDrag;
    private float _prevDrag;
    void PickupObject(GameObject pickObj)
    {
        EventManager.Instance.invokePickupEventGlobal(); // event for scalableObject.cs
        
        _heldObj = pickObj;
        _heldObjRb = pickObj.GetComponent<Rigidbody>();
        
        _boxCollider = _heldObj.GetComponent<BoxCollider>();
        
        _heldObj.tag = "pickedUp";
        _heldObj.transform.parent = this.transform;
        
        _prevDrag = _heldObjRb.drag;
        _prevAngularDrag = _heldObjRb.angularDrag;
         
        _heldObjRb.drag = 2;
        _heldObjRb.angularDrag = 1;
        _heldObjRb.useGravity = false;
    }
    
    public void DropObject(){  
        if(_heldObj == null || _heldObjRb == null) { return; }
        _heldObj.tag = "repeat";
        _heldObj.transform.parent = null;
        
        _heldObjRb.drag = _prevDrag;
        _heldObjRb.angularDrag = _prevAngularDrag;
        _heldObjRb.useGravity = true;
        
        _heldObj = null;
        _heldObjRb = null;
    }

    public LayerMask ground;
    private BoxCollider _boxCollider;
    private List<Collider> otherColliders = new List<Collider>();
    void MoveObject()
    {
        float moveDistance = Vector3.Distance(holdArea.position, _heldObj.transform.position);

        if (moveDistance > pickupRange)
        {
            DropObject();
            return;
        }

        Vector3 moveDirection = (holdArea.position - _heldObj.transform.position);
        
        RaycastHit hit;
        if (Physics.Raycast(_heldObj.transform.position, moveDirection, out hit, moveDistance * 4, ground))
        {
            Vector3 nearestPoint = hit.collider.ClosestPoint(hit.point);
            moveDirection = (nearestPoint - _heldObj.transform.position);
            _heldObjRb.AddForce(moveDirection * pickupForce, ForceMode.Acceleration);
            return;
        }
        
        Collider[] col = Physics.OverlapBox(_heldObj.transform.position + _boxCollider.center, _boxCollider.size / 2, Quaternion.identity, ground);
    
        otherColliders.Clear();
        foreach (Collider c in col)
        {
            if (!c.CompareTag(_heldObj.tag))
            {
                otherColliders.Add(c);
            }
        }

        if (otherColliders.Count > 0)
        {
            Vector3 nearestPoint = otherColliders[0].ClosestPoint(holdArea.position);
            
            moveDirection = (nearestPoint - _heldObj.transform.position);
            _heldObjRb.AddForce(moveDirection * pickupForce, ForceMode.Force);
            return;
        }
        _heldObj.transform.position = holdArea.position;
    }

    
    void rotateObject()
    {
        if (_heldObjRb.constraints != RigidbodyConstraints.FreezeRotation)
        {
            Vector3 rotationEulerAngles = _heldObjRb.transform.rotation.eulerAngles;

            // Round the angles to the nearest multiple of 90 degrees
            rotationEulerAngles.x = Mathf.Round(rotationEulerAngles.x / 90) * 90;
            rotationEulerAngles.y = Mathf.Round(rotationEulerAngles.y / 90) * 90;
            rotationEulerAngles.z = Mathf.Round(rotationEulerAngles.z / 90) * 90;

            // Smoothly interpolate between the current rotation and the target rotation
            Quaternion targetRotation = Quaternion.Euler(rotationEulerAngles);
            float rotationSpeed = 25f; // Adjust the rotation speed as needed
            _heldObjRb.MoveRotation(Quaternion.Lerp(_heldObjRb.rotation, targetRotation, Time.deltaTime * rotationSpeed));
        }
    }
} 