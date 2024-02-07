using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Dictionary<GameObject, List<GameObject>> doorEnemies = new Dictionary<GameObject, List<GameObject>>();
    public List<GameObject> tutorialHitboxes = new List<GameObject>();

    public GameObject mobsSpawner1;

    private GameObject loseMenu;
    private GameObject winMenu;

    private GameObject player;
    public Transform spawn1;

    public GameObject door1;
    public GameObject door2;
    public GameObject door3;
    public GameObject door4;
    public GameObject door5;
    public GameObject door6;
    public GameObject door7;
    public GameObject door8;
    public GameObject door9;
    public GameObject door10;

    [Header("Boss")]
    public bool isBossOneDed;
    public bool isBossTwoDed;
    public bool isBossThreeDed;

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
        winMenu = GameObject.Find("WinMenu");
        player = GameObject.FindWithTag("Player");
        DontDestroyOnLoad(player);
        mobsSpawner1.SetActive(false);
    }

    void Start()
    {
        loseMenu.SetActive(false);
        winMenu.SetActive(false);
        SpawnPlayer();
    }

    void Update()
    {
        CheckTutorialHitboxes();
    }

    void InitializeDoors()
    {
        doorEnemies.Add(door1, new List<GameObject>()); 
        doorEnemies.Add(door2, new List<GameObject>());
        doorEnemies.Add(door3, new List<GameObject>());
        doorEnemies.Add(door4, new List<GameObject>());
        doorEnemies.Add(door5, new List<GameObject>());
        doorEnemies.Add(door6, new List<GameObject>());
        doorEnemies.Add(door7, new List<GameObject>());
        doorEnemies.Add(door8, new List<GameObject>());
        doorEnemies.Add(door9, new List<GameObject>());
        doorEnemies.Add(door10, new List<GameObject>());
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

    void SpawnPlayer() 
    { 
        player.transform.position = spawn1.position;
    }

    public void PlayerDied()
    {
        loseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void PlayerWon()
    {
        winMenu.SetActive(true);
        Time.timeScale = 0;
    }

    void CheckTutorialHitboxes()
    {
        if (tutorialHitboxes.Count <= 0)
        {
            
            EnableMobsSpawner();
        }
    }

    void EnableMobsSpawner()
    {
        mobsSpawner1.SetActive(true);
    }
}
