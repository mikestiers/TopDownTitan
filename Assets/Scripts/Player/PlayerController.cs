using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 7.5f;
    public float touchMoveSpeed = 15f;
    public Rigidbody2D rb;
    public CircleCollider2D playerCollider;
    public AudioClip deathSound;
    public WeaponInventory inventory;
    private Vector3 targetPosition;
    Vector3 moveDirection;

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (Input.GetMouseButtonDown(1))
        {
            inventory.currentWeapon.Fire(playerCollider);
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
                touchPosition.z = Camera.main.nearClipPlane;

                // Cast a ray from the camera to the touch position
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector2.zero);

                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    // Check if the ray hit the player's collider
                    if (hit.collider.gameObject == gameObject)
                    {
                        // Handle the touch based on its phase
                        switch (touch.phase)
                        {
                            case TouchPhase.Began:
                                // Fire the weapon when the touch starts
                                inventory.currentWeapon.Fire(playerCollider);
                                break;

                            case TouchPhase.Moved:
                                // Set the player's position to the touch position
                                Vector3 targetPosition = touchPosition;
                                targetPosition.z = transform.position.z; // Keep the original z-axis position
                                transform.position = targetPosition;

                                // Fire the weapon when the touch moves
                                inventory.currentWeapon.Fire(playerCollider);
                                break;

                            case TouchPhase.Stationary:
                                // Fire the weapon when the touch does not move
                                inventory.currentWeapon.Fire(playerCollider);

                                break;
                        }
                    }
                }
            }
        }
    }

    void FixedUpdate()
    {
        // This is only for keyboard / gamepad
        rb.velocity = new Vector3(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed, 0);
    }

    public void TakeDamage(float damage)
    {
        if (GameManager.singleton.shields > 0)
        {
            GameManager.singleton.shields -= 1;
            HUD.singleton.UpdateShields(GameManager.singleton.shields);
        }
        else if (GameManager.singleton.shields <= 0)
        {
            AudioManager.singleton.PlaySoundEffect(deathSound);
            Time.timeScale = 0f;
            HUD.singleton.gameOverMenuCanvas.SetActive(true);
        }
    }

    public void AddShield(int shields)
    {
        Debug.Log($"Shield Pickup");
        shields = GameManager.singleton.shields + shields;
        HUD.singleton.UpdateShields(shields);
    }
}