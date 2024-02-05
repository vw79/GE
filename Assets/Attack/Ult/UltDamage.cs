using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltDamage : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        EnemyAI enemy = other.GetComponent<EnemyAI>();

        if (other.gameObject.layer == enemyLayer)
        {
            enemy.takeDamage(500);
        }
    }
}
