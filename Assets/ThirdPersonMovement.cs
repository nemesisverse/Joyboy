using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public float turnsmoothtime = 0.1f;
    public float speed = 6f;
    float turnsmoothvelocity;


    float verticalvelocity;
    Vector3 dampingVelocity;

    public bool isGrounded;
    public float verticalVel;
    private Vector3 moveVector;

    private Plane playerPlane;

    public void PlayerMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");


        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        if (direction.magnitude > 0.1f)
        {
            float targetangle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetangle, ref turnsmoothvelocity, turnsmoothtime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetangle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);


            // if (verticalvelocity < 0 && controller.isGrounded)
            // {
            //     verticalvelocity = Physics.gravity.y * Time.deltaTime;
            // }
            // else
            // {
            //     verticalvelocity += Physics.gravity.y * Time.deltaTime;
            // }
        }
    }

    private void Start()
    {
        playerPlane = new Plane(Vector3.up, Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
      
        PlayerMovement();
        LookMouse();
        isGrounded = controller.isGrounded;
        if (isGrounded)
        {
            verticalVel -= 0;
        }
        else
        {
            verticalVel -= 1;
        }
        moveVector = new Vector3(0, verticalVel * .2f * Time.deltaTime, 0);
        controller.Move(moveVector);



    }
    void LookMouse()
    { // Get the ray from the camera to the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Variable to store the distance from the ray origin to the intersection point
        float hitDistance;

        // Check if the ray intersects with the plane
        if (playerPlane.Raycast(ray, out hitDistance))
        {
            // Get the intersection point
            Vector3 targetPoint = ray.GetPoint(hitDistance);

            // Calculate the direction from the player to the target point
            Vector3 direction = targetPoint - transform.position;

            // Set the y component to zero to keep the player facing on the XZ plane only
            direction.y = 0;

            // Calculate the rotation needed to look at the target point
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Apply the rotation to the player instantly
            transform.rotation = targetRotation;
        }
    }
}
