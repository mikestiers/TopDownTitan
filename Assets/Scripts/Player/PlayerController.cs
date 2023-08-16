using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 7.5f;
    public float touchMoveSpeed = 15f;
    public Rigidbody rb;
    public Weapon weapon;
    public BoxCollider playerCollider;

    Vector3 moveDirection;

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (Input.GetMouseButtonDown(1))
        {
            weapon.Fire(playerCollider);
        }

        moveDirection = new Vector3(moveX, moveY, 0).normalized;

        // Check if there are any touches on the screen
        if (Input.touchCount > 0)
        {
            // Iterate through all the touches on the screen
            for (int i = 0; i < Input.touchCount; i++)
            {
                // Get information about the current touch
                Touch touch = Input.GetTouch(i);

                // Convert the touch position to world coordinates
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                touchPosition.z = 0; // Set z-axis to 0 to prevent movement in the z-axis

                // Cast a ray from the camera to the touch position
                // because I used 3D instead of 2D, otherwise collider2d has an overlappoint method
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out hit))
                {
                    // Check if the ray hit the player's collider
                    if (hit.collider.gameObject == gameObject)
                    {
                        // Handle the touch based on its phase
                        switch (touch.phase)
                        {
                            case TouchPhase.Began:

                                // Fire the weapon when the touch starts
                                weapon.Fire(playerCollider);
                                break;

                            case TouchPhase.Moved:

                                // Fire the weapon when the touch moves
                                weapon.Fire(playerCollider);
                                break;

                            case TouchPhase.Stationary:

                                // Set the player's position to the touch position
                                Vector3 targetPosition = touchPosition;
                                targetPosition.z = transform.position.z; // Keep the original z-axis position
                                transform.position = targetPosition;

                                // Fire the weapon when the touch does not move
                                weapon.Fire(playerCollider);

                                break;
                        }
                    }
                }
            }
        }
    }

    private void FixedUpdate()
    {
        // This is only for keyboard / gamepad
        rb.velocity = new Vector3(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed, 0);
    }
}