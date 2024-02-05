using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FireAttackCollider : MonoBehaviour

{
    void OnTriggerEnter(Collider other)
    {
        int enemyLayer = LayerMask.NameToLayer("Enemy");

        if (other.gameObject.layer == enemyLayer)
        {
            other.GetComponent<EnemyAI>().takeDamage(50);
            /*StartCoroutine(DealDamageOverTime(other.GetComponent<EnemyAI>(), 50, 5));*/
        }
    }

    /*IEnumerator DealDamageOverTime(EnemyAI enemy, int damage, float duration)
    {
        if (enemy == null) yield break;

        // Temporarily disable the NavMeshAgent to "stun" the enemy
        NavMeshAgent navMeshAgent = enemy.GetComponent<NavMeshAgent>();
        Animator animator = enemy.GetComponent<Animator>();

        if (navMeshAgent != null && navMeshAgent.enabled)
        {
            animator.Play("Halt");
            navMeshAgent.isStopped = true;
            navMeshAgent.enabled = false;
            
        }

        float elapsed = 0;
        float damageInterval = 1.0f;
        while (elapsed < duration)
        {
            enemy.takeDamage(damage);
            yield return new WaitForSeconds(damageInterval);
            elapsed += damageInterval;
        }

        // Re-enable the NavMeshAgent after the damage period is over
        if (navMeshAgent != null && !navMeshAgent.enabled)
        {
            navMeshAgent.enabled = true;
            navMeshAgent.isStopped = false;
        }
    }*/
}
