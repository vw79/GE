using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int curHealth = 100;

    public float healthBarLength;

    void Start()
    {
        healthBarLength = Screen.width / 2;
    }

    void Update()
    {
        AdjustCurrentHealth(0);
    }

    private void OnGUI()
    {
        GUI.Box(new Rect(10, 40, Screen.width / 2 / (maxHealth / curHealth), 20), curHealth + "/" + maxHealth);
    }

    public void AdjustCurrentHealth(int adj)
    {
        curHealth += adj;

        if (curHealth < 0)
            curHealth = 0;

        if (curHealth > maxHealth)
            curHealth = maxHealth;

        if (maxHealth < 1)
            maxHealth = 1;

        healthBarLength = (Screen.width / 2) * (curHealth / (float)maxHealth);
    }
}
