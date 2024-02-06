using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBoss: MonoBehaviour
{
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject bossBullet;
    [SerializeField] private GameObject door;

    private void Start()
    {
        boss.SetActive(false);
        bossBullet.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            boss.SetActive(true);
            bossBullet.SetActive(true);
            door.SetActive(true);
        }
    }
}

