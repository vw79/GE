using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public MaterialData playerMaterialData;
    private Dictionary<GameObject, List<GameObject>> doorEnemies = new Dictionary<GameObject, List<GameObject>>();
    public List<GameObject> tutorialHitboxes = new List<GameObject>();

    private StateManager stateManager;
    private VisualEffect iceEffect;
    private ParticleSystem fireEffect;
    private SkinnedMeshRenderer playerRenderer;
    private GameObject sword;
    public GameObject mobsSpawner1;

    private GameObject loseMenu;
    private GameObject winMenu;

    private GameObject player;
    private PlayerHealthSystem playerHealth;
    private Transform spawn1;

    private GameObject boss3;
    private BossController bossController;

    [Header("Boss")]
    public bool isBossThreeDed;

    [Header("SFX")]
    public AudioSource openDoor;

    private GameObject pauseMenu;
    private PlayerController playerController;
    private PlayerCombat playerCombat;
    public bool isPaused;
    public bool isTutorial;


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
        pauseMenu = GameObject.Find("PauseMenu");
        player = GameObject.FindWithTag("Player");
        DontDestroyOnLoad(player);
        stateManager = GameObject.Find("StateManager").GetComponent<StateManager>();
        playerRenderer = player.GetComponentInChildren<SkinnedMeshRenderer>();
        iceEffect = GameObject.Find("IceAttack").GetComponent<VisualEffect>();
        fireEffect = GameObject.Find("FireAttack").GetComponent<ParticleSystem>();
        sword = GameObject.FindWithTag("Sword");
        playerHealth = player.GetComponent<PlayerHealthSystem>();
        spawn1 = GameObject.FindWithTag("InitialSpawn").transform;
        
        playerController = player.GetComponent<PlayerController>();
        playerCombat = player.GetComponent<PlayerCombat>();        
        mobsSpawner1.SetActive(false);

        loseMenu.SetActive(false);
        winMenu.SetActive(false);
        pauseMenu.SetActive(false);
        SpawnPlayer();


        boss3 = GameObject.FindWithTag("Boss3");
        bossController = boss3.GetComponent<BossController>();

        isBossThreeDed = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        CheckTutorialHitboxes();

        if (bossController.currentHealth <= 0)
        {
           isBossThreeDed = true;
           StartCoroutine(PlayerWon());
        }
    }

    private void RestorePlayerMaterials()
    {
        if (playerMaterialData != null)
        {          
            if (playerRenderer != null)
            {
                playerRenderer.materials = playerMaterialData.originalMaterials;
            }
        }
    }


    void InitializeDoors()
    {
        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");

        // Iterate through each door and add it to the dictionary
        foreach (GameObject door in doors)
        {
            if (!doorEnemies.ContainsKey(door))
            {
                doorEnemies.Add(door, new List<GameObject>());
            }
        }
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
        openDoor.Play();
        door.SetActive(false); 
    }

    void SpawnPlayer() 
    { 
        player.transform.position = spawn1.position;
    }

    public void PlayerDied()
    {
        loseMenu.SetActive(true);
        
        playerRenderer.materials = new Material[0];
        Time.timeScale = 0;
    }

    IEnumerator PlayerWon()
    {
        yield return new WaitForSeconds(5f);
        winMenu.SetActive(true);
        Time.timeScale = 0;
    }

    void CheckTutorialHitboxes()
    {   
        if (tutorialHitboxes != null)
        { 
            if (tutorialHitboxes.Count <= 0)
            {
                EnableMobsSpawner();
            } 
        }
    }

    void EnableMobsSpawner()
    {
        if (mobsSpawner1 != null)
        {
            mobsSpawner1.SetActive(true); 
        }
    }

    public void RestartGame()
    {
        player.SetActive(true);
        iceEffect.Stop();
        fireEffect.Stop();
        RestorePlayerMaterials();
        sword.SetActive(true);
        player.transform.position = spawn1.position;
        playerHealth.ResetHealth();
        stateManager.currentState = StateManager.State.State1;

        loseMenu.SetActive(false);
        winMenu.SetActive(false);
        pauseMenu.SetActive(false);

        Time.timeScale = 1f;

        playerCombat.enabled = true;
        playerController.enabled = true;

        isPaused = false;
    }

    #region PauseMenu
    public void PauseGame()
    {
        if (!isTutorial)
        {
            pauseMenu.SetActive(true);
            playerCombat.enabled = false;
            playerController.enabled = false;
            Time.timeScale = 0f;
            isPaused = true;
        }    
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        playerCombat.enabled = true;
        playerController.enabled = true;
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    #endregion
}
