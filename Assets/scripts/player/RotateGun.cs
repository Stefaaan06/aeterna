using UnityEngine;

/*
 * Rotates Grappling Gun towards Grappling point
 * @author Stefaaan
 * @version 1.0.1 - added documentation
 * not used in game - used with grappling gun, probably needs rework
 */
public class RotateGun : MonoBehaviour {

    public GrapplingGun grappling;  //grappling script
    private Quaternion desiredRotation;     //desired gun rotation
    private float rotationSpeed = 5f;   //speed of gun rotation

    /*
     * Called every Frame
     */
    void Update() {
        if (!grappling.IsGrappling()) {     //if the Player isnt grappling
            desiredRotation = transform.parent.rotation;    //rotates gun forward
        }
        else {      //if the player is grappling
            desiredRotation = Quaternion.LookRotation(grappling.GetGrapplePoint() - transform.position);    //calculates rotation direction
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime * rotationSpeed);      //rotates gun
    }

}
