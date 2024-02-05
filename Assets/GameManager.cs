using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private List<GameObject> enemies = new List<GameObject>();

    // Reference to the door you want to unlock
    public GameObject doorToUnlock;

    private void Awake()
    {
        // Singleton pattern to ensure only one GameManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
    }

    public void EnemyDefeated(GameObject enemy)
    {
        // Remove the enemy from the list
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
            CheckEnemies();
        }

        Debug.Log(enemies.Count);
    }

    void CheckEnemies()
    {
        // If there are no more enemies, unlock the door
        if (enemies.Count == 0)
        {
            UnlockDoor();
            Debug.Log("All enemies defeated!");
        }
    }

    void UnlockDoor()
    {
        // Implement door unlocking logic here
        doorToUnlock.SetActive(false); // Example of unlocking by deactivating the door GameObject
        Debug.Log("Door Unlocked!");
    }
}

