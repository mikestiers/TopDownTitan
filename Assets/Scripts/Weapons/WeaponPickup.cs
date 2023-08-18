using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public Weapon newWeaponPrefab; // Assign the new weapon prefab in the Inspector
    public Sprite newWeaponIcon; // Assign an icon for the HUD

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Get the player controller
            PlayerController playerController = other.GetComponent<PlayerController>();

            // Check if the player controller exists
            if (playerController != null)
            {
                // Change the active weapon
                playerController.ChangeWeapon(newWeaponPrefab, newWeaponIcon);

                // Destroy the weapon pickup
                Destroy(gameObject);
            }
        }
    }
}
