using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage;
    BoxCollider triggerBox;
    public PlayerCombat playerCombat; 

    void Start()
    {
        triggerBox = GetComponent<BoxCollider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (playerCombat.isAttacking && other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
        }
    }

    public void EnableTrigger()
    {
        triggerBox.enabled = true;
    }

    public void DisableTrigger()
    {
        triggerBox.enabled = false;
    }
}
