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
        health -= damage;
        if (health <= 0)
        {
            if (Random.value < dropChance)
            {
                DropPickup();
            }
            if (destructionEffect != null)
            {
                destructionEffect.Play();
                StartCoroutine(DestroyAfterEffect(destructionEffect.main.duration));
            }
            DestroyEnemy();
            Camera.main.GetComponent<CameraShake>().TriggerShake();
            AudioManager.singleton.PlaySoundEffect(deathSound);
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

    //void Update()
    //{
    //    if (Time.realtimeSinceStartup >= fireCoolDown)
    //    {
    //        fireCoolDown = Time.realtimeSinceStartup + fireCoolDown;
    //        weapon.Fire(enemyCollider);
    //    }

    //    transform.Translate(moveSpeed * Time.deltaTime * downDirection);
    //}
}
