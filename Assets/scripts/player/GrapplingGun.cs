using UnityEngine;
using System.Collections;


public class GrapplingGun : MonoBehaviour {
    private LineRenderer lr;

    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public Transform gunTip, camPos, player;
    private float maxDistance = 200f;
    private SpringJoint joint;
    public bool grappling =false;


    Vector3 Distance;
    [SerializeField] float DragSpeed;
    [SerializeField] Rigidbody rb;

    void Awake() {
        lr = GetComponent<LineRenderer>();
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            StartGrapple();
        }
        else if (Input.GetMouseButtonUp(0)) {
            StopGrapple();
        }
    }

    //Called after Update
    void LateUpdate() {
        DrawRope();
    }

    /// <summary>
    /// Call to start grapple
    /// </summary>
    void StartGrapple() {
        
        
        RaycastHit hit;
        if (Physics.Raycast(camPos.position, camPos.forward, out hit, maxDistance, whatIsGrappleable)) {
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            //The distance grapple will try to keep from grapple point. 
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            //Adjust these values to fit your game.
            
            joint.damper = 7f;
            joint.massScale = 4.5f;
            StartCoroutine(wait() ) ;
            grappling = true;

            lr.positionCount = 2;
            currentGrapplePosition = gunTip.position;
        }
    }


    /// <summary>
    /// call to stop grapple
    /// </summary>
    void StopGrapple() {  

        grappling = false;
        lr.positionCount = 0;
        Destroy(joint);
    }

    private Vector3 currentGrapplePosition;
    
    /// <summary>
    /// Call to draw to grapple rope (temp) 
    /// </summary>
    void DrawRope() {
        if (!joint) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 8f);
        
        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, currentGrapplePosition);
    }
    
    public bool IsGrappling() {
        return joint != null;
    }

    public Vector3 GetGrapplePoint() {
        return grapplePoint;
}
public IEnumerator wait(){
    joint.spring = 100f;
    yield return new WaitForSeconds(0.1f);
    joint.spring = 4.5f;
}
}
