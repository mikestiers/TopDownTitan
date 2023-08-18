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
            DestroyEnemy();
            AudioManager.singleton.PlaySoundEffect(deathSound);
        }
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
