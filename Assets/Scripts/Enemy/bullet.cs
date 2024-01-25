using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public HealthSystem playerHealth;
    public float bulletDamage;
    private void Start()
    {
       
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            playerHealth.TakeDamage(bulletDamage);
        }
        Destroy(gameObject);
    }

}
