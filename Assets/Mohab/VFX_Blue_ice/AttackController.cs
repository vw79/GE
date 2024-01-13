using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public GameObject attackVFXPrefab;
    public float damageAmount = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            // Trigger attack VFX only when 'B' key is pressed
            GameObject attackVFX = Instantiate(attackVFXPrefab, transform.position, Quaternion.identity);

            // Destroy the VFX after a certain time (adjust the second parameter as needed)
            Destroy(attackVFX, 1.0f);

            // Check for enemies in the vicinity
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5f);

            // Damage enemies
            foreach (Collider collider in hitColliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    // Apply damage to the enemy
                    EnemyHealth enemyHealth = collider.GetComponent<EnemyHealth>();

                    if (enemyHealth != null)
                    {
                        enemyHealth.TakeDamage(damageAmount);
                    }
                }
            }
        }
    }

}
