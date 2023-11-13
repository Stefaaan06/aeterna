using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

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

    [Header("Refferences")]
    [SerializeField] private PlayerMovement playerMovement;

    private static GameObject m_heldObj = null;
    private Rigidbody _heldObjRB;
    private Vector3 _scale;

   
    [Header("Physicis Parameters")]
    [SerializeField] private float pickupRange;
    [SerializeField] private float pickupForce;
    

    float _objMass;
    private void Update(){
        if(Input.GetKeyDown(KeyCode.E)){
            if(m_heldObj == null){    
                RaycastHit hit;
                //shoots Raycast to find Object within the PickupRange and having the correct layer
                if(Physics.Raycast(cam.position, cam.forward, out hit, pickupRange, pickupLayer)){
                    PickupObject(hit.transform.gameObject); 
                }
            }else{
                DropObject();
            }
        }
        if(m_heldObj == null) return;
        MoveObject();

    }

    public void crouch(bool crouch)
    {
        if(m_heldObj == null) return;
        
        if(crouch){
            if(m_heldObj.transform.localScale == _scale){
                m_heldObj.transform.localScale = m_heldObj.transform.localScale * 2;
                holdArea.transform.localPosition = new Vector3(0, 0, 4.5f);
            }   
        }else{
            m_heldObj.transform.localScale = _scale;
            holdArea.transform.localPosition = new Vector3(0, 0, 2.5f);

        }
    }
    

    float _prevAngularDrag;
    private float _prevDrag;
    void PickupObject(GameObject pickObj)
    {
        
        m_heldObj = pickObj;
        _heldObjRB = pickObj.GetComponent<Rigidbody>();

        
        m_heldObj.tag = "pickedUp";
        m_heldObj.transform.parent = this.transform;
        
        _prevDrag = _heldObjRB.drag;
        _prevAngularDrag = _heldObjRB.angularDrag;
         
        _heldObjRB.drag = 2;
        _heldObjRB.angularDrag = 1;
        _heldObjRB.useGravity = false;
        
        if(!playerMovement.crouching){
            _scale = m_heldObj.transform.localScale;
        }else{
            _scale = m_heldObj.transform.localScale / 2;
        }
    }
    
    public void DropObject(){  
        if(m_heldObj == null || _heldObjRB == null) { return; }
        m_heldObj.tag = "repeat";
        m_heldObj.transform.parent = null;
        
        m_heldObj = null;
            
        _heldObjRB.drag = _prevDrag;
        _heldObjRB.angularDrag = _prevAngularDrag;
        _heldObjRB.useGravity = true;
        
        
        _heldObjRB = null;
    }

    public LayerMask ground;
    void MoveObject()
    {
        Vector3 moveDirection = (holdArea.position - m_heldObj.transform.position);
        float moveDistance = Vector3.Distance(holdArea.position, m_heldObj.transform.position);
        
        RaycastHit hit;
        if (Physics.Raycast(m_heldObj.transform.position, moveDirection, out hit, moveDistance * 2, ground))
        {
            // Adjust the position to the nearest non-intersecting point
            _heldObjRB.AddForce(moveDirection * pickupForce, ForceMode.Force);
            return;
        }
        
        Collider[] col = Physics.OverlapBox(m_heldObj.transform.position, m_heldObj.transform.localScale / 2, Quaternion.identity, ground);

        if (col.Length > 1)
        {
            _heldObjRB.AddForce(moveDirection * pickupForce);
            return;
        }
        m_heldObj.transform.position = holdArea.position;
        
        
        
        if (_heldObjRB.constraints != RigidbodyConstraints.FreezeRotation)
        {
            Vector3 rotationEulerAngles = m_heldObj.transform.rotation.eulerAngles;

            // Round the angles to the nearest multiple of 90 degrees
            rotationEulerAngles.x = Mathf.Round(rotationEulerAngles.x / 90) * 90;
            rotationEulerAngles.y = Mathf.Round(rotationEulerAngles.y / 90) * 90;
            rotationEulerAngles.z = Mathf.Round(rotationEulerAngles.z / 90) * 90;

            // Smoothly interpolate between the current rotation and the target rotation
            Quaternion targetRotation = Quaternion.Euler(rotationEulerAngles);
            float rotationSpeed = 20f; // Adjust the rotation speed as needed
            _heldObjRB.MoveRotation(Quaternion.Lerp(_heldObjRB.rotation, targetRotation, Time.deltaTime * rotationSpeed));
        }
    }



} 