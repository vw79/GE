using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Dictionary<GameObject, List<GameObject>> doorEnemies = new Dictionary<GameObject, List<GameObject>>();

    private GameObject loseMenu;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        InitializeDoors();

        loseMenu = GameObject.Find("LoseMenu");
    }

    void Start()
    {
        loseMenu.SetActive(false);
    }

    void InitializeDoors()
    {
        GameObject door1 = GameObject.Find("CheckDoor1");
        GameObject door2 = GameObject.Find("CheckDoor2");

        doorEnemies.Add(door1, new List<GameObject>()); 
        doorEnemies.Add(door2, new List<GameObject>());
    }

    public void RegisterEnemy(GameObject enemy, GameObject door)
    {
        if (doorEnemies.ContainsKey(door))
        {
            doorEnemies[door].Add(enemy);
        }
    }

    public void EnemyDefeated(GameObject enemy)
    {
        foreach (var door in doorEnemies.Keys)
        {
            if (doorEnemies[door].Contains(enemy))
            {
                doorEnemies[door].Remove(enemy);
                CheckEnemies(door);
                break; 
            }
        }
    }

    void CheckEnemies(GameObject door)
    {
        if (doorEnemies[door].Count == 0)
        {
            UnlockDoor(door);
        }
    }

    void UnlockDoor(GameObject door)
    {
        door.SetActive(false); 
    }

    // Game Over
    public void PlayerDied()
    {
        loseMenu.SetActive(true);
        Time.timeScale = 0;
    }
}
