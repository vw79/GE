using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UltMeter : MonoBehaviour
{
    [SerializeField] private Image[] ultBarImages;
    private int currentKillCount = 0;
    private const int maxKillCount = 10;
    public bool canUlt;

    void Start()
    {
        ResetUltBar();
    }   

    public void IncrementKillCount()
    {
        if (currentKillCount < maxKillCount)
        {
            currentKillCount++;
            UpdateUltBar();
        }

        if (currentKillCount == maxKillCount)
        {
            EnableUltimate();
        }
    }

    private void UpdateUltBar()
    {
        for (int i = 0; i < ultBarImages.Length; i++)
        {
            ultBarImages[i].enabled = i < currentKillCount;
        }
    }

    private void EnableUltimate()
    {
        canUlt = true;
    }

    public void ResetUltBar()
    {
        currentKillCount = 0;
        UpdateUltBar();
        canUlt = false;
    }
}

