using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] private Rigidbody rb;

    
    
    [Header("Movment")]
    public Transform playerCam;
    public Transform orientation;
    public Transform headCheck;
    public Transform groundCheck;
    public float moveSpeed = 4500;
    public float maxSpeed = 20;
    public float counterMovement = 0.175f;
    public float maxAngle = 50;
    [SerializeField ] public bool grounded, onSlope;
    public LayerMask whatIsGround;
    public float groundDistance;

    
    //movement privates & consts
    private float _xRotation;
    private float _sensitivity = 60f; 
    private float _sensMultiplier = 1f;
    private readonly float _threshold = 0.01f;
    private const float _origGround = 0.6f;
    private bool _jumped, _readyToJump = true;
    private float _jumpCooldown = 0.25f;

    
    [Header("Crouching,Sliding & Jumping")]
    private Vector3 _crouchScale = new Vector3(1, 1, 1);
    public Vector3 playerScale;
    public float slideForce = 400;
    public float slideCounterMovement = 0.2f;
    public bool canUncrouch;
    public LayerMask CanUncrouch;
    public float jumpForce = 550f;

    //Input
    float x, y;
    [SerializeField ] bool jumping, crouching = false;
    
    //MovementStates
    [SerializeField ] public bool canSlide = true, canJump = true, canMove = true, moving = false;

    [Header("References")]
    [SerializeField] WallRunning wallRun;
    [SerializeField] GrapplingGun grappling;
    [SerializeField] private ObjectPickup objectPickup;
    [SerializeField] private playerFX playerFX;
    public playerPauseMenu pauseMenu;
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        
        playerScale = this.transform.localScale;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 6 && _timeInAir > 10)
        {
            playerFX.playCollisionSound(collision);
        }
    }
    private int _timeInAir;
    void airTime()
    {
        if (!grounded)
        {
            _timeInAir++;
        }
        else
        {
            if(_timeInAir > 0)
            {
                _timeInAir -= 5;
            }
        }
    }
    private void FixedUpdate() {
        Movement();
    }
    void Update(){
        if(pauseMenu.paused) return;
        CheckIfGrounded();
        MyInput();
        airTime();
        Look();
    }


    private void MyInput()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        if (rb.velocity.magnitude > 1)
        {
            moving = true;
        }
        else
        {
            moving = false;
        }

        if (canJump)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Jump();
            }
        }
    }


    private void Jump()
    {
        if (grounded && _readyToJump)
        {
            playerFX.Jump();
            jumping = true;
            _readyToJump = false;

            //jump force
            rb.AddForce(Vector2.up * (jumpForce * 1.5f));
            rb.AddForce(Vector3.up * (jumpForce * 0.5f));
            
            Vector3 vel = rb.velocity;
            if (rb.velocity.y < 0.5f)
                rb.velocity = new Vector3(vel.x, 0, vel.z);
            else if (rb.velocity.y > 0) 
                rb.velocity = new Vector3(vel.x, vel.y / 2, vel.z);           
            
            Invoke(nameof(ResetJump), _jumpCooldown);
        }
    }
    
    public bool extraGravity = true;
    public float extraGravityStrength = 10f;
    public float maxRbSpeed = 50;
    private void Movement() {
        if(!canMove) return;
        //Extra gravity
        if (extraGravity)
        {
            rb.AddForce(Vector3.down * (Time.deltaTime * extraGravityStrength));
        }
        
        //Find actual velocity relative to where player is looking
        Vector2 mag = FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;

        //Counteract sliding and sloppy movement
        CounterMovement(x, y, mag);
        //Set max speed
        float maxSpeed = this.maxSpeed;
        if (rb.velocity.magnitude > maxRbSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxRbSpeed;
        }
        
        // If sliding down a ramp, add force down so player stays grounded and also builds speed
        if (crouching && grounded && _readyToJump) {
           rb.AddForce(Vector3.down * (Time.deltaTime * 1000)); //CHANGES HERE
        }
        
        
        //Some multipliers
        float multiplier = 1f, multiplierV = 1f;
        
        
        /*
         * Essentially obsolete. No changes are made to movement speed
         */
        
        // Movement in air
        if (!grounded) {
            multiplier = 1f; 
            multiplierV = 1f;
        }
        // Movement while sliding
        if(crouching){
            multiplier = 1f;
            multiplierV = 1f;
        }

        if (wallRun.isWallRunning)
        {
            multiplier = 1f;
            multiplierV = 1f;
        }

        if (rb.velocity.magnitude > maxSpeed)
        {
            multiplier = 0.5f;
            multiplierV = 0.5f;
        }

        if (onSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * (moveSpeed * Time.deltaTime * multiplier * multiplierV * 1.5f));
        }
        else
        {
            //Apply forces to move player
            if (pauseMenu.paused)
            {
                moving = false;
                return; 
            }
            rb.AddForce(orientation.transform.forward * (y * moveSpeed * Time.deltaTime * multiplier * multiplierV));
            rb.AddForce(orientation.transform.right * (x * moveSpeed * Time.deltaTime * multiplier));   
        }
    }
    
    private void ResetJump() {
        _readyToJump = true;
        jumping = false;
    }
    
    private float desiredX;
    private void Look() {   
        float mouseX = Input.GetAxis("Mouse X") * _sensitivity * Time.fixedDeltaTime * _sensMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * _sensitivity * Time.fixedDeltaTime * _sensMultiplier;

        //Find current look rotation
        Vector3 rot = playerCam.transform.localRotation.eulerAngles;
        desiredX = rot.y + mouseX;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        //Perform the rotations
        playerCam.transform.localRotation = Quaternion.Euler(_xRotation, desiredX, wallRun.Tilt);
        orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);
    }

    private void CounterMovement(float x, float y, Vector2 mag) {
        if (!grounded || jumping) return;

        //Slow down sliding
        if (crouching) {
           rb.AddForce(-rb.velocity.normalized * (moveSpeed * Time.deltaTime * slideCounterMovement));
            return;
        }

        //Counter movement
        if (Math.Abs(mag.x) > _threshold && Math.Abs(x) < 0.05f || (mag.x < -_threshold && x > 0) || (mag.x > _threshold && x < 0)) {
            rb.AddForce(orientation.transform.right * (moveSpeed * Time.deltaTime * -mag.x * counterMovement));
        }
        if (Math.Abs(mag.y) > _threshold && Math.Abs(y) < 0.05f || (mag.y < -_threshold && y > 0) || (mag.y > _threshold && y < 0)) {
            rb.AddForce(orientation.transform.forward * (moveSpeed * Time.deltaTime * -mag.y * counterMovement));
        }
        
        //Limit diagonal running. This will also cause a full stop if sliding fast and un-crouching, so not optimal.
        if (Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) > maxSpeed) {
            float fallspeed = rb.velocity.y;
            Vector3 n = rb.velocity.normalized * maxSpeed;
            rb.velocity = new Vector3(n.x, fallspeed, n.z);
        }
    }
    public Vector2 FindVelRelativeToLook() {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitue = rb.velocity.magnitude;
        float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);
        
        return new Vector2(xMag, yMag);
    }
    

    private void CheckIfGrounded(){
        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, whatIsGround);
        if (grounded)
        {
            CheckIfSlope();
        }
    }

    private RaycastHit _hit = new RaycastHit();
    private float _angle;
    private void CheckIfSlope()
    {
        Physics.Raycast(groundCheck.position, Vector3.down, out _hit, 10);
        _angle = Vector3.Angle(Vector3.up, _hit.normal);
        onSlope = _angle < maxAngle && _angle != 0;
    }

    private Vector3 _moveDir;
    private Vector3 GetSlopeMoveDirection()
    {
        _moveDir = orientation.forward * y + orientation.right * x;
        return Vector3.ProjectOnPlane(_moveDir, _hit.normal).normalized;
    }
    

}