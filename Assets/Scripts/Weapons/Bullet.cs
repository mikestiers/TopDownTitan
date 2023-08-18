using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damageAmount = 20.0f;
    public int scoreValue = 100;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damageAmount);
                GameManager.singleton.IncreaseScore(scoreValue);
            }

            Destroy(gameObject); // Destroy the bullet
        }
        else if (other.CompareTag("ScreenPerimiter"))
        {
            Destroy(gameObject); // Destroy the bullet
        }
        else if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (GameManager.singleton.shields >= 0)
            {
                Destroy(gameObject); // Destroy the bullet
                player.TakeDamage(damageAmount);
            }
        }
    }
}
