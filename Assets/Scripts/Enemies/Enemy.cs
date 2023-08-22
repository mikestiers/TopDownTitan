using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float fireCoolDown = 2.0f;
    public string enemyType;
    public string attackPattern;
    public float moveSpeed = 5.0f;
    public float health;
    public AudioClip deathSound;
    public Weapon weapon;
    public CapsuleCollider enemyCollider;
    public Transform playerTransform;
    public WeaponPickup droppedWeaponPrefab;
    public float dropChance = 0.25f; // 25% chance of dropping a pickup
    public ParticleSystem destructionEffect;
    private bool isDead = true;
    public Vector3 downDirection => -transform.up;

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ScreenPerimiter")
        {
            Destroy(gameObject);
        }

        if (other.tag == "Player")
        {
            PlayerController player = other.GetComponent<PlayerController>();
            player.TakeDamage(1);
        }
    }

    protected virtual void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void TakeDamage(float damage)
    {
        if (health <= 0) return; // while particle effect plays don't take damage

        health -= damage;
        if (health <= 0)
        {
            isDead = true; // Assuming you have an isDead flag to disable movement, etc.

            // Disable the collider
            Collider enemyCollider = GetComponent<Collider>();
            if (enemyCollider)
            {
                enemyCollider.enabled = false;
            }

            // Disable movement
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            if (Random.value < dropChance)
            {
                DropPickup();
            }

            if (destructionEffect != null)
            {
                destructionEffect.Play();
                Camera.main.GetComponent<CameraShake>().TriggerShake();
                AudioManager.singleton.PlaySoundEffect(deathSound);
                StartCoroutine(DestroyAfterEffect(destructionEffect.main.duration));
            }
            //DestroyEnemy();
        }
    }

    IEnumerator DestroyAfterEffect(float delay)
    {
        yield return new WaitForSeconds(delay);
        DestroyEnemy();
    }


    void DropPickup()
    {
        Instantiate(droppedWeaponPrefab, transform.position, Quaternion.identity);
    }

    void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
