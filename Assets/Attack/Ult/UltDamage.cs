using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltDamage : MonoBehaviour
{
    private EnemyAI enemyAI;
    private BossController bossController;

    void OnTriggerEnter(Collider other)
    {
        int enemyLayer = LayerMask.NameToLayer("Enemy");

        if (other.gameObject.layer == enemyLayer)
        {
            if (enemyAI = other.GetComponent<EnemyAI>())
            {
                enemyAI.takeDamage(500);
            }
            else if (bossController = other.GetComponent<BossController>())
            {
                bossController.takeDamage(500);
            }
        }
    }
}
