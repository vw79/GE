using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBoss: MonoBehaviour
{
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject bossBullet;
    [SerializeField] private GameObject door;

    [Header("Boss")]
    public BossController bossController;
    public bool isRed;
    public bool isBlue;
    public bool isGreen;


    private void Start()
    {
        bossController.isRed = isRed;
        bossController.isBlue = isBlue;
        bossController.isGreen = isGreen;
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

