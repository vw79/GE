using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceAttackCollider: MonoBehaviour
{
    private Ice ice;
    [SerializeField] private float iceDamage = 50;

    private void Awake()
    {
        ice = GameObject.FindWithTag("Player").GetComponent<Ice>();
    }

    void OnTriggerEnter(Collider other)
    {
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        EnemyAI enemy = other.GetComponent<EnemyAI>();

        if (other.gameObject.layer == enemyLayer)
        {
            StartCoroutine(ice.SlowEnemy(enemy));
            enemy.takeDamage(iceDamage);
        }
    }
}
