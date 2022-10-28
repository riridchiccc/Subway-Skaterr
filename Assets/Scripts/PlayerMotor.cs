using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private const float LANE_DISTANCE = 3.0f;
    private const float TURN_SPEED = 0.05f;

    //Movement
    private CharacterController controller;
    private float verticalVelocity;
    private float gravity = 12.0f;
    private float jumpForce = 4.0f;
    private float speed = 7.0f;
    private int desiredLane = 1; //0 = left, 1 = middle, 2 = right


    private void Start()
    {
        controller = GetComponent<CharacterController>(); 
    }

    private void Update()
    {
        //Gather input for which lane we should be
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            MoveLane(false);
        if (Input.GetKeyUp(KeyCode.RightArrow))
            MoveLane(true);
        

        //Calculate where we should be in the future
        Vector3 targetPosition = transform.position.z * Vector3.forward;
        if (desiredLane == 0)
            targetPosition += Vector3.left * LANE_DISTANCE;
        else if (desiredLane == 2)
            targetPosition += Vector3.right * LANE_DISTANCE;

        //to calculate our move delta
        Vector3 moveVector = Vector3.zero;
        moveVector.x = (targetPosition - transform.position).normalized.x * speed;

        //Calculate y
        if(true) // if Grounded
        moveVector.y = -0.1f;
        moveVector.z = speed;

        //to control the penguin
        controller.Move(moveVector * Time.deltaTime);

        //to rotate the penguin to the direction of th lane he's switching to
        Vector3 dir = controller.velocity;
        if (dir != Vector3.zero)
        {
            dir.y = 0;
            transform.forward = Vector3.Lerp(transform.forward, dir, TURN_SPEED);
        }
    }
    private void MoveLane(bool goingRight)
    {
        desiredLane += (goingRight) ? 1 : -1;
        desiredLane = Mathf.Clamp(desiredLane, 0, 2);

    }
}