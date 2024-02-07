using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBoss: MonoBehaviour
{
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject bossBullet;
    [SerializeField] private GameObject doorBeforeBoss;
    [SerializeField] private GameObject doorAfterBoss;

    [Header("Boss")]
    public BossController bossController;
    [SerializeField] public bool isRed;
    [SerializeField] public bool isBlue;
    [SerializeField] public bool isGreen;


    private void Start()
    {
        bossController.isRed = isRed;
        bossController.isBlue = isBlue;
        bossController.isGreen = isGreen;
        boss.SetActive(false);
        bossBullet.SetActive(false);
    }

    private void Update()
    {
        if (bossController.isDead && doorAfterBoss)
        {
            doorAfterBoss.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            boss.SetActive(true);
            bossBullet.SetActive(true);
            doorBeforeBoss.SetActive(true);
        }
    }
}

