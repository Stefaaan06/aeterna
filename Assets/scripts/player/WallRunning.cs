using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Manages player wallrunning ability
 * @author Stefaaan
 * @version 1.1.0 - reworked system
 */
public class WallRunning : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Transform orientation;
    [SerializeField] private PlayerMovement _playerMovement;

    [Header("Detection")]
    [SerializeField] private float wallDistance = .5f;
    [SerializeField] private float sphereRadius = .5f;

    [Header("Wall Running")]
    [SerializeField] private float wallRunGravity;
    [SerializeField] private float wallRunJumpForce;
    [SerializeField] private float normalJumpForce;

    [Header("Camera")]
    [SerializeField] private Camera cam;
    [SerializeField] private float fov;
    [SerializeField] private float camTilt;
    [SerializeField] private float camTiltTime;

    public float Tilt { get; private set; }

    private bool _wallLeft = false;
    private bool _wallRight = false;
    
    private bool _jumpLeft = false;
    private bool _jumpRight = false;

    private RaycastHit _leftWallHit;
    private RaycastHit _rightWallHit;

    [SerializeField] Rigidbody rb;

    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public bool isWallRunning = false;
    public bool isWallJumping = false;


    void Start()
    {
        whatIsGround = _playerMovement.whatIsGround;
    }
    

    private Vector3 _pos;
    private Vector3 _orientation;
    void CheckWall()
    {
        _pos = orientation.transform.position;
        _orientation = orientation.right;
        //Checks if there are wallrunnable walls next to the player
        _wallLeft = Physics.Raycast(_pos, -_orientation, out _leftWallHit, wallDistance, whatIsWall);
        if(_wallLeft) return;
        _wallRight = Physics.Raycast(_pos, _orientation, out _rightWallHit, wallDistance, whatIsWall);
        if(_wallRight) return;
        _jumpLeft = Physics.Raycast(_pos, -_orientation, out _leftWallHit, wallDistance, whatIsGround);
        if(_jumpLeft) return;
        _jumpRight = Physics.Raycast(_pos, _orientation, out _rightWallHit, wallDistance, whatIsGround);
    }

    private void Update()
    {
        if (_playerMovement.grounded)
        {
            isWallJumping = false;
            StopWallRun();
            return;
        }
        CheckWall();    //checks wall
        
        
        if (_wallLeft || _wallRight)
        {
            StartWallRun();
        }
        else
        {
            StopWallRun();
        }
        
        
        if (_jumpLeft || _jumpRight)
        {
            isWallJumping = true;
            wallJump();
        }
    }

    void wallJump()
    {
        //If player wants to jump of wall
        if (Input.GetKeyDown(KeyCode.Space) && !_playerMovement.grounded)
        {
            if (_jumpLeft)
            {
                //jumps of wall 
                Vector3 wallRunJumpDirection = transform.up + _leftWallHit.normal;
                rb.AddForce(wallRunJumpDirection * normalJumpForce * 100, ForceMode.Force);
            }
            else if (_jumpRight)
            {
                //jumps of wall
                Vector3 wallRunJumpDirection = transform.up + _rightWallHit.normal;
                rb.AddForce(wallRunJumpDirection * normalJumpForce * 100, ForceMode.Force);
            }
        }
    }
    
    void StartWallRun()
    {

        rb.useGravity = false;  //stops using gravity
        rb.AddForce(Vector3.down * wallRunGravity, ForceMode.Force);    //creates simulated gravity

        //tilts camera
        if (_wallLeft)
            Tilt = Mathf.Lerp(Tilt, -camTilt, camTiltTime * Time.deltaTime);
        else if (_wallRight)
            Tilt = Mathf.Lerp(Tilt, camTilt, camTiltTime * Time.deltaTime);
        isWallRunning = true;
        //If player wants to jump of wall
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_wallLeft)
            {
                //jumps of wall 
                Vector3 wallRunJumpDirection = transform.up + _leftWallHit.normal;
                rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 100, ForceMode.Force);
            }
            else if (_wallRight)
            {
                //jumps of wall
                Vector3 wallRunJumpDirection = transform.up + _rightWallHit.normal;
                rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 100, ForceMode.Force);
            }
        }
    }
    
    void StopWallRun()
    {
        //resets values

        rb.useGravity = true;
        Tilt = Mathf.Lerp(Tilt, 0, camTiltTime * Time.deltaTime);

        isWallRunning = false;   
    }
}