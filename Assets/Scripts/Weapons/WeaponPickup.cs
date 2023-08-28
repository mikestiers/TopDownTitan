using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public Weapon weapon;
    public int ammo;

    private void Awake()
    {
        //GetComponent<MeshRenderer>().material.color = newWeaponPrefab.color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Get the player controller
            PlayerController playerController = other.GetComponent<PlayerController>();

            // Check if the player controller exists
            if (playerController != null)
            {
                if (tag == "PickupWeapon")
                {
                    // Change the active weapon and add it to inventory
                    playerController.inventory.AddWeapon(weapon);
                }

                if (tag == "PickupShield")
                {
                    playerController.AddShield(1);
                }

                // Destroy the weapon pickup
                Destroy(gameObject);
            }
        }
    }
}
