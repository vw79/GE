using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceAttackCollider: MonoBehaviour
{
    private Ice ice;
    [SerializeField] private float iceDamage = 50;
    private EnemyAI enemyAI;
    private BossController bossController;

    private void Awake()
    {
        ice = GameObject.FindWithTag("Player").GetComponent<Ice>();
    }

    void OnTriggerEnter(Collider other)
    {
        int enemyLayer = LayerMask.NameToLayer("Enemy");

        if (other.gameObject.layer == enemyLayer)
        {
            if (enemyAI = other.GetComponent<EnemyAI>())
            {
                StartCoroutine(ice.SlowEnemy(enemyAI));
                enemyAI.takeDamage(iceDamage);
            }
            else if (bossController = other.GetComponent<BossController>())
            {
                bossController.takeDamage(iceDamage);
            }
        }
    }
}
