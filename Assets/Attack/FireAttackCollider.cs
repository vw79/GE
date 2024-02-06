using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FireAttackCollider : MonoBehaviour

{
    [SerializeField] private float fireDamage = 10;
    [SerializeField] private float dotDuration = 4;
    private EnemyAI enemyAI;
    private BossController bossController;

    void OnTriggerEnter(Collider other)
    {
        int enemyLayer = LayerMask.NameToLayer("Enemy");

        if (other.gameObject.layer == enemyLayer)
        {


            if (other.gameObject.layer == enemyLayer)
            {
                if (enemyAI = other.GetComponent<EnemyAI>())
                {
                    other.GetComponent<EnemyAI>().takeDamage(fireDamage);
                    StartCoroutine(MobsDealDamageOverTime(other.GetComponent<EnemyAI>(), fireDamage, dotDuration));

                }
                else if (bossController = other.GetComponent<BossController>())
                {
                    bossController.takeDamage(fireDamage);
                    StartCoroutine(BossDealDamageOverTime(other.GetComponent<BossController>(), fireDamage, dotDuration));
                }
            }
        }

        IEnumerator MobsDealDamageOverTime(EnemyAI enemy, float damage, float duration)
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

        IEnumerator BossDealDamageOverTime(BossController enemy, float damage, float duration)
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
}