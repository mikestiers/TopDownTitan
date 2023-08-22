using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaCanon : Weapon
{
    private int currentBullets = 100;
    public override void Fire(Collider shooter)
    {
        if (currentBullets > 0)
        {
            if (Time.time > nextFireTime)
            {
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                Physics.IgnoreCollision(bullet.GetComponent<Collider>(), shooter, true);
                bullet.GetComponent<Rigidbody>().AddForce(firePoint.up * fireForce); //, ForceMode.Impulse);
                AudioManager.singleton.PlaySoundEffect(fireSound);

                // Set the next fire time
                nextFireTime = Time.time + fireRate;
                currentBullets--;
            }
        }
    }
}
