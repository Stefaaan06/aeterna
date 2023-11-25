using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portalCamera : MonoBehaviour
{
    	private Transform playerCamera;
    	public Transform portal;
    	public Transform otherPortal;
	    

	    private void Start()
	    {
		    
		    playerCamera = GameObject.FindGameObjectWithTag("playerCam").transform;
	    }

	    void Update () {

		    
		    Vector3 playerOffsetFromPortal = playerCamera.position - otherPortal.position;
    		transform.position = portal.position + playerOffsetFromPortal;
    
		    
    		float angularDifferenceBetweenPortalRotations = Quaternion.Angle(portal.rotation, otherPortal.rotation);
    
    		Quaternion portalRotationalDifference = Quaternion.AngleAxis(angularDifferenceBetweenPortalRotations, Vector3.up);
    		Vector3 newCameraDirection = portalRotationalDifference * playerCamera.forward;
    		transform.rotation = Quaternion.LookRotation(newCameraDirection, Vector3.up);
			    
    	}
}
