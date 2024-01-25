using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltDamage : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        int enemyLayer = LayerMask.NameToLayer("Enemy");

        if (other.gameObject.layer == enemyLayer)
        {
            Destroy(other.gameObject);
        }
    }
}
