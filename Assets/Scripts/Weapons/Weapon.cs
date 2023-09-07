using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public int weaponId;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public AudioClip fireSound;
    public Sprite icon;
    public Color color;
    public float fireForce = 2000f;
    public float fireRate = 0.1f;
    public int currentBullets;
    [System.NonSerialized] public float nextFireTime = 0.0f;

    public abstract void Fire(Collider2D shooter);
}
