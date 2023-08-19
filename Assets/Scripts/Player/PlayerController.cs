using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 7.5f;
    public float touchMoveSpeed = 15f;
    public Rigidbody rb;
    public Weapon startingWeaponPrefab;
    public Weapon weapon;
    public BoxCollider playerCollider;
    public AudioClip deathSound;
    private Vector3 targetPosition;
    Vector3 moveDirection;

    private void Start()
    {
        // Instantiate the starting weapon and set it as the active weapon
        weapon = Instantiate(startingWeaponPrefab, transform);
    }

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
                touchPosition.z = Camera.main.nearClipPlane;
                //touchPosition.z = 0; // Set z-axis to 0 to prevent movement in the z-axis

                // Cast a ray from the camera to the touch position
                // because I used 3D instead of 2D, otherwise collider2d has an overlappoint method
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(touch.position);

                float raycastDistance = 100f; // Maximum raycast distance
                float sphereRadius = 1.0f; // Radius of the sphere
                if (Physics.SphereCast(ray, sphereRadius, out hit, raycastDistance))
                {
                //    if (Physics.Raycast(ray, out hit, 1000f)) // remove 1000f probably
                //{
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
                                // Set the player's position to the touch position
                                Vector3 targetPosition = touchPosition;
                                targetPosition.z = transform.position.z; // Keep the original z-axis position
                                transform.position = targetPosition;

                                // Fire the weapon when the touch moves
                                weapon.Fire(playerCollider);
                                break;

                            case TouchPhase.Stationary:
                                // Fire the weapon when the touch does not move
                                weapon.Fire(playerCollider);

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

    public void ChangeWeapon(Weapon newWeaponPrefab, Sprite newWeaponIcon)
    {
        Debug.Log($"Change Weapon: {newWeaponPrefab} prefab, {newWeaponIcon} icon");
        if (weapon != null)
            Destroy(weapon.gameObject);

        weapon = Instantiate(newWeaponPrefab, transform);

        HUD.singleton.AddWeapon(newWeaponPrefab, newWeaponIcon);
    }
}
