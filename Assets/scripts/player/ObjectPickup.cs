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

    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip[] clips;
    
    
    private static GameObject _heldObj = null;
    private Rigidbody _heldObjRb;
    private Vector3 _scale;

   
    [Header("Physics Parameters")]
    [SerializeField] private float pickupRange;
    [SerializeField] private float pickupForce;
    
    private bool _pickedUp = false;
    

    float _objMass;
    private void Update(){
        if(Input.GetKeyDown(KeyCode.E)){
            if(_heldObj == null){    
                RaycastHit hit;
                //shoots Raycast to find Object within the PickupRange and having the correct layer
                if(Physics.Raycast(cam.position, cam.forward, out hit, pickupRange, pickupLayer)){
                    source.pitch = Random.Range(1.1f, 1.3f);
                    source.PlayOneShot(clips[0], 0.8f);
                    
                    PickupObject(hit.transform.gameObject); 
                    return;
                }
            }else{
                source.pitch = Random.Range(0.7f, 0.9f);
                source.PlayOneShot(clips[0], 0.8f);
                
                DropObject();
                return;
            }
        }
        
        if(!_pickedUp) return;
        rotateObject();
        MoveObject();
    }
    
    
    

    float _prevAngularDrag;
    private float _prevDrag;
    void PickupObject(GameObject pickObj)
    {
        _pickedUp = true;
        EventManager.Instance.invokePickupEventGlobal(); // event for scalableObject.cs
        
        _heldObj = pickObj;
        _heldObjRb = pickObj.GetComponent<Rigidbody>();
        
        _boxCollider = _heldObj.GetComponent<BoxCollider>();
        
       _heldObj.GetComponent<cube>().enabled = false;
        
        _heldObj.layer = 11;
        _heldObj.tag = "pickedUp";

        _heldObj.transform.parent = this.transform;
        
        _prevDrag = _heldObjRb.drag;
        _prevAngularDrag = _heldObjRb.angularDrag;
         
        _heldObjRb.drag = 2;
        _heldObjRb.angularDrag = 1;
        _heldObjRb.useGravity = false;
    }
    
    public void DropObject(){  
        if(!_pickedUp) { return; }
        _pickedUp = false;
        _heldObj.transform.parent = null;
        
        _heldObjRb.drag = _prevDrag;
        _heldObjRb.angularDrag = _prevAngularDrag;
        _heldObjRb.useGravity = true;
        
        _heldObj.layer = 9;
        _heldObj.tag = "repeat";

       _heldObj.GetComponent<cube>().enabled = true;
        
        _heldObj = null;
        _heldObjRb = null;
    }

    RaycastHit hit;
    bool raycast()
    {
        if(Physics.Raycast(cam.position, cam.transform.forward, out hit, 10,ground))
        {
            return true;
        }
        return false;
    }
    
    public LayerMask ground;
    private BoxCollider _boxCollider;
    private List<Collider> otherColliders = new List<Collider>();
    void MoveObject()
    {
        float moveDistance = Vector3.Distance(holdArea.localPosition, _heldObj.transform.localPosition);
        
        if (moveDistance > pickupRange)
        {
            DropObject();
            return;
        }
        
        Vector3 moveDirection = (holdArea.position - _heldObj.transform.position);

        bool raycastHit = raycast();
        if (raycastHit)
        {
            Vector3 nearestPoint = hit.collider.ClosestPoint(hit.point);
            moveDirection = (nearestPoint - _heldObj.transform.position);
            _heldObjRb.AddForce(moveDirection * (pickupForce * 100 * Time.deltaTime), ForceMode.Acceleration);
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
            moveDirection = (holdArea.position - _heldObj.transform.position);
            _heldObjRb.AddForce(moveDirection * (pickupForce * 100 * Time.deltaTime), ForceMode.Acceleration);

            return;
        }
        
        _heldObjRb.MovePosition(holdArea.position);
    }


    
    void rotateObject()
    {
        if (_heldObjRb.constraints != RigidbodyConstraints.FreezeRotation)
        {
            Vector3 rotationEulerAngles = _heldObjRb.transform.rotation.eulerAngles;

            rotationEulerAngles.x = Mathf.Round(rotationEulerAngles.x / 90) * 90;
            rotationEulerAngles.y = Mathf.Round(rotationEulerAngles.y / 90) * 90;
            rotationEulerAngles.z = Mathf.Round(rotationEulerAngles.z / 90) * 90;

            Quaternion targetRotation = Quaternion.Euler(rotationEulerAngles);

            _heldObjRb.MoveRotation(Quaternion.Lerp(_heldObjRb.rotation, targetRotation,  25));
        }
    }
} 