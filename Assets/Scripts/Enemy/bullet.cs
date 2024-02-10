using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public PlayerHealthSystem playerHealth;
    public float bulletDamage;
    public float shootForce;
    Rigidbody rb;
    private void Start()
    {
       rb= GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        //rb.AddForce(transform.forward * shootForce, ForceMode.Impulse);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (playerHealth.isDead)
        {
            return;
        }

        if (other.CompareTag("Player") && playerHealth.enabled)
        {
            playerHealth.TakeDamage(bulletDamage);
        }
        Destroy(gameObject);
    }

}
