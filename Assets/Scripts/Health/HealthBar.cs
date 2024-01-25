using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar: MonoBehaviour
{
    public Slider healthSlider;
    public Slider easeHealthSlider;
    public float health;
    private float learpSpeed = 0.02f;
    public HealthSystem healthSystem;

    // Start is called before the first frame update
    void Start()
    {
        health = healthSystem.GetHealth();
    }

    // Update is called once per frame
    void Update()
    {
        if (healthSlider.value != health)
        {
            healthSlider.value = health;
        }

        if (healthSlider.value != easeHealthSlider.value)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, health, learpSpeed);
        }
    }

    public void SetHealth(float health)
    {
        this.health = health;
    }
}
