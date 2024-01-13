using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;  // Maximum health of the enemy
    private float currentHealth;    // Current health of the enemy
    public void TakeDamage(float amount)
    {
        // Implement your damage logic here
        // For example, decrease enemy health or destroy the enemy
        // You can customize this method based on your game's requirements
        Debug.Log("Enemy took damage: " + amount);
    }
    void Start()
    {
        // Initialize current health to max health when the enemy is spawned
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void InitializeHealth(float initialHealth)
    {
        maxHealth = initialHealth;
        currentHealth = initialHealth;
    }
    void DefeatEnemy()
    {
        // Implement any actions when the enemy is defeated, e.g., play death animation, destroy the enemy, etc.
        Debug.Log("Enemy defeated!");
        Destroy(gameObject);
    }

    // Public method to set the initial health of the enemy
    public void SetInitialHealth(float initialHealth)
    {
        maxHealth = initialHealth;
        currentHealth = initialHealth;
    }

    // Public method to get the current health of the enemy
    public float GetCurrentHealth()
    {
        return currentHealth;
    }
}
