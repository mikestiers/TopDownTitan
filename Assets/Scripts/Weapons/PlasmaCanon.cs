using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaCanon : Weapon
{
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
