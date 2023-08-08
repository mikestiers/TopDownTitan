using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireForce = 2000f;

    public void Fire(Collider shooter)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Physics.IgnoreCollision(bullet.GetComponent<Collider>(), shooter, true);
        bullet.GetComponent<Rigidbody>().AddForce(firePoint.up * fireForce); //, ForceMode.Impulse);
    }
}
