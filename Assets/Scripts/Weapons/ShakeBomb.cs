using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShakeBomb : Weapon
{
    public float shakeDetectionThreshold = 2.0f;
    public float shakeInterval = 0.5f;
    private float timeSinceLastShake = 0.0f;

    void Update()
    {
        timeSinceLastShake += Time.deltaTime;

        // Check if enough time has passed since the last shake
        if (timeSinceLastShake > shakeInterval)
        {
            // Calculate the device's acceleration
            Vector3 acceleration = Input.acceleration;

            // Calculate the magnitude of the acceleration
            float accelerationMagnitude = acceleration.magnitude;

            // Check if the acceleration magnitude exceeds the shake detection threshold
            if (accelerationMagnitude > shakeDetectionThreshold)
            {
                // A shake has been detected, drop shakebomb
                //Fire(player);

                // Reset the time since the last shake
                timeSinceLastShake = 0.0f;
            }
        }
    }

    public override void Fire(Collider2D shooter)
    {
        if (currentBullets > 0)
        {
            if (Time.time > nextFireTime)
            {
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), shooter, true);
                bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce); //, ForceMode.Impulse);
                AudioManager.singleton.PlaySoundEffect(fireSound);

                // Set the next fire time
                nextFireTime = Time.time + fireRate;
                currentBullets--;
            }
        }
        else
        {
            WeaponInventory.singleton.RemoveWeapon(this.weaponId);
        }
    }
}
