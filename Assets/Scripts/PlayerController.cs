using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public Animator teslaAnimation;
    private CharacterController controller;
    private Vector3 direction;
    public float forwardSpeed;

    private int desiredLane = 1;//0:Left 1:Middle 2:Right
    public float laneDistance = 3.2f; //the distance between two lanes
    private float maxSpeed = 17;

    public float jumpForce;
    private float Gravity = -15;
    private bool isSliding;
    private float slidingTimer = 1.2f;
    private bool timerIsRunning = false;

    private float groundRayDistance = 1;
    private RaycastHit _slopeHit;
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        direction.z = forwardSpeed;

        if (forwardSpeed <= maxSpeed)
            forwardSpeed += 0.1f * Time.deltaTime;

        if (controller.isGrounded)
        {
            animator.SetBool("isJumping", false);
            direction.y = -1;
            if (/*Input.GetKeyDown(KeyCode.UpArrow)*/ SwipeManager.swipeUp)
            {
                animator.SetBool("isJumping", true);
                Jump();                
            }
        }
        else
        {
            direction.y += Gravity * Time.deltaTime;
        }
        if (OnSteepSlope())
            SteepSlopeMovement();
        

        //Gather inputs on which we should be
        if (/*Input.GetKeyDown(KeyCode.RightArrow)*/ SwipeManager.swipeRight)
        {
            desiredLane++;
            if (desiredLane == 3)
                desiredLane = 2;
        }
        if (/*Input.GetKeyDown(KeyCode.LeftArrow)*/ SwipeManager.swipeLeft)
        {
            desiredLane--;
            if (desiredLane == -1)
                desiredLane = 0;
        }
        if (/*Input.GetKeyDown(KeyCode.DownArrow)*/SwipeManager.swipeDown)
        {
            if(timerIsRunning == false)
                Slide();
        }
        if (timerIsRunning)
        {
            if(slidingTimer > 0)
            {
                slidingTimer -= Time.deltaTime;
            }
            else
            {
                animator.SetBool("isSliding", false);
                controller.height = 2;
                timerIsRunning = false;                
                slidingTimer = 1.2f;
                Gravity = -15;
            }
        }
        //Calculate where we should be in the future
        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;

        if (desiredLane == 0)
            targetPosition += Vector3.left * laneDistance;
        else if (desiredLane == 2)
            targetPosition += Vector3.right * laneDistance;

        //if (transform.position != targetPosition)
        //{
        //    Vector3 diff = targetPosition - transform.position;
        //    Vector3 moveDir = diff.normalized * 30 * Time.deltaTime;
        //    if (moveDir.sqrMagnitude < diff.magnitude)
        //        controller.Move(moveDir);
        //    else
        //        controller.Move(diff);
        //}

        controller.Move(direction * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, targetPosition, 20 * Time.fixedDeltaTime);
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Obstacle")
        {
            PlayerManager.gameOver = true;
            //FindObjectOfType<AudioManager>().PlaySound("GameOver");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Obstacle")
            PlayerManager.gameOver = true;
    }
    private void Jump()
    {
        Gravity = -15;
        direction.y = jumpForce;

        if (animator.GetBool("isSliding"))
        {
            animator.SetBool("isSliding", false);
            timerIsRunning = false;
            slidingTimer = 1.2f;
            isSliding = false;
            controller.height = 2;            
        }
    }

    private void Slide()
    {
            timerIsRunning = true;
            Gravity = -50;
            controller.height = 1;
            animator.SetBool("isJumping", false);
            animator.SetBool("isSliding", true);              
                 
    }
    //private IEnumerator Slide()
    //{
    //    isSliding = true;
    //    animator.SetBool("isJumping", false);
    //    animator.SetBool("isSliding", true);
    //    controller.center = new Vector3(0, 0, 0);
    //    controller.height = 1;
    //    Gravity = -50;
    //    yield return new WaitForSeconds(0.8f);

    //    isSliding = false;
    //    controller.center = new Vector3(0, 0, 0);
    //    controller.height = 2;
    //    Gravity = -15;
    //    animator.SetBool("isSliding", false);
    //}


    private bool OnSteepSlope()
    {
        if (!controller.isGrounded) return false;

        if(Physics.Raycast(transform.position, Vector3.down, out _slopeHit, (controller.height / 2) + groundRayDistance))
        {
            float slopeAngle = Vector3.Angle(_slopeHit.normal, Vector3.up);
            if (slopeAngle > controller.slopeLimit) return true;
        }
        return false;
    }
    private void SteepSlopeMovement()
    {
        Vector3 slopeDirection = Vector3.up - _slopeHit.normal * Vector3.Dot(Vector3.up, _slopeHit.normal);
        float slideSpeed = forwardSpeed + Time.deltaTime;

        direction.z *= -slideSpeed;
        direction.y -= _slopeHit.point.y;
    }
}
