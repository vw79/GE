using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FireAttackCollider : MonoBehaviour

{
    [SerializeField] private float fireDamage = 10;
    [SerializeField] private float dotDuration = 4;

    void OnTriggerEnter(Collider other)
    {
        int enemyLayer = LayerMask.NameToLayer("Enemy");

        if (other.gameObject.layer == enemyLayer)
        {
            other.GetComponent<EnemyAI>().takeDamage(10);
            StartCoroutine(DealDamageOverTime(other.GetComponent<EnemyAI>(), fireDamage, dotDuration));
        }
    }

    IEnumerator DealDamageOverTime(EnemyAI enemy, float damage, float duration)
    {
        if (enemy == null) yield break;

        float elapsed = 0;
        float damageInterval = 1f;
        while (elapsed < duration)
        {
            enemy.takeDamage(damage);
            yield return new WaitForSeconds(damageInterval);
            elapsed += damageInterval;
        }
    }
}
