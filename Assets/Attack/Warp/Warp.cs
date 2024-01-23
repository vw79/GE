using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour
{
    public LayerMask enemyLayer;
    public Material[] targetMaterials;

    public float radius = 10f; // The radius within which to find enemies
    public float safeDistance = 2f; // Distance to maintain from the enemy after teleporting
    public float warpDuration = 2f; // Duration of the warp animation
    public Color gizmoColor = Color.red; // Color of the Gizmo

    private Transform playerTransform;
    private Transform furthestEnemy; // Updated to store the furthest enemy
    private Vector3 warpStartPosition;
    private Vector3 warpTargetPosition;
    private float warpStartTime;

    public Animator animator;

    void Start()
    {
        playerTransform = transform;
    }

    void Update()
    {
        // Handle the warp animation
        if (warpStartTime > 0f)
        {
            float warpProgress = (Time.time - warpStartTime) / warpDuration;
            if (warpProgress < 1f)
            {
                playerTransform.position = Vector3.Lerp(playerTransform.position, warpTargetPosition, warpProgress);
            }
            else
            {
                playerTransform.position = warpTargetPosition;
                warpStartTime = 0f;
            }
        }
    }

    public void StartWarp()
    {
        Collider[] hitColliders = Physics.OverlapSphere(playerTransform.position, radius, enemyLayer);
        float maxDistance = 0f;

        foreach (var hitCollider in hitColliders)
        {
            float distance = Vector3.Distance(playerTransform.position, hitCollider.transform.position);
            if (distance > maxDistance)
            {
                maxDistance = distance;
                furthestEnemy = hitCollider.transform;
            }
        }

        if (furthestEnemy != null)
        {
            // Check if the furthestEnemy has one of the specified tags
            string enemyTag = furthestEnemy.gameObject.tag;
            if (enemyTag == "Red" || enemyTag == "Green" || enemyTag == "Blue")
            {
                // Apply the corresponding material
                Material enemyMaterial = null;

                if (enemyTag == "Red")
                {
                    enemyMaterial = targetMaterials[0];
                }
                else if (enemyTag == "Green")
                {
                    enemyMaterial = targetMaterials[1];
                }
                else if (enemyTag == "Blue")
                {
                    enemyMaterial = targetMaterials[2];
                }

                if (enemyMaterial != null)
                {
                    // Store the original material
                    Material originalMaterial = furthestEnemy.GetComponentInChildren<SkinnedMeshRenderer>().material;

                    // Apply the new material
                    furthestEnemy.GetComponentInChildren<SkinnedMeshRenderer>().material = enemyMaterial;

                    // Start a coroutine to revert to the original material after a delay
                    StartCoroutine(RevertMaterialAfterDelay(furthestEnemy.gameObject, originalMaterial, 1.5f)); 
                }
            }
        }

        animator.Play("Warp");
        StartCoroutine(WaitAnim());
    }

    IEnumerator WaitAnim()
    {
        yield return new WaitForSeconds(1.1f);
        WarpToEnemy(); // Call the updated function
    }

    private void WarpToEnemy()
    {
        Vector3 enemyPosition = furthestEnemy.position;
        Vector3 directionToEnemy = (enemyPosition - playerTransform.position).normalized;

        // Rotate the player to face the enemy's position
        playerTransform.LookAt(enemyPosition);

        warpStartPosition = playerTransform.position;
        warpTargetPosition = enemyPosition - directionToEnemy * (safeDistance + 1f); // Adjusted for safe distance
        warpStartTime = Time.time;
    }

    // Coroutine to revert the material after a delay
    private IEnumerator RevertMaterialAfterDelay(GameObject targetObject, Material originalMaterial, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (targetObject != null)
        {
            Renderer renderer = targetObject.GetComponentInChildren<SkinnedMeshRenderer>();
            if (renderer != null)
            {
                renderer.material = originalMaterial;
            }
        }
    }

    // Draw a gizmo in the editor to visualize the attack radius
    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}