using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject target;
    public float attackTimer;
    public float coolDown;

    void Start()
    {
        attackTimer = 0;
        coolDown = 2.0f;
    }

    void Update()
    {
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }

        if (attackTimer < 0)
        {
            attackTimer = 0;
        }

        if (Input.GetKeyUp(KeyCode.F))
        {
            Attack();
            attackTimer = coolDown;
        }
    }

    private void Attack()
    {
        float distance = Vector3.Distance(target.transform.position, transform.position);
        Vector3 dir = (target.transform.position - transform.position).normalized;
        float direction = Vector3.Dot(dir, transform.forward);

        if (distance < 2.5f)
        {
            if(direction > 0)
            {
                //EnemyHealth enemyHealth = target.GetComponent<EnemyHealth>();
                //enemyHealth.AdjustCurrentHealth(-10);
            }
        }
    }
}
