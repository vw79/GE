using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour
{
    public float radius = 10f; // The radius within which to find enemies
    public float safeDistance = 2f; // Distance to maintain from the enemy after teleporting
    public Color gizmoColor = Color.red; // Color of the Gizmo

    void Update()
    {
        // Check if the 'E' key is pressed
        if (Input.GetKeyDown(KeyCode.E))
        {
            WarpToEnemy();
        }
    }

    void WarpToEnemy()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        float maxDistance = 0f;
        Transform furthestEnemy = null;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    furthestEnemy = hitCollider.transform;
                }
            }
        }

        if (furthestEnemy != null)
        {
            Vector3 enemyPosition = furthestEnemy.position;
            Vector3 directionToEnemy = (enemyPosition - transform.position).normalized;
            Vector3 warpPosition = enemyPosition - directionToEnemy * (safeDistance + 1f); // Adjusted for safe distance

            // Warp the player to the position
            transform.position = warpPosition;
        }
    }

    // Draw a gizmo in the editor to visualize the attack radius
    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}


