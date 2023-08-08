using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Destroy(other.gameObject);
            //Destroy(gameObject);
        }
        if (other.tag == "ScreenPerimiter")
        {
            Destroy(gameObject);
        }
    }
}
