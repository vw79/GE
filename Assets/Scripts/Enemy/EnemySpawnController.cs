using UnityEngine;
using UnityEngine.UI;

public class EnemySpawnController : MonoBehaviour
{
    public GameObject redRangeEnemyPrefab;
    public GameObject greenRangeEnemyPrefab;
    public GameObject blueRangeEnemyPrefab;
    public GameObject RedEnemyPrefab;
    public GameObject BlueEnemyPrefab;
    public GameObject GreenEnemyPrefab;
    public Transform[] spawnPoints;
    public int numberOfEnemies;
    bool isEntered;
    public bool isRed;
    public bool isBlue;
    public bool isGreen;
    [HideInInspector]
    bool Melee;
    [SerializeField]public EnemyAI enemy;
    private void Awake()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")&&!isEntered) 
        {
            SpawnEnemies();
            isEntered = true;
        }
    }
    public void SpawnEnemies()
        {
            if (isRed)
            {
                SpawnColorEnemies(RedEnemyPrefab, redRangeEnemyPrefab);
            }

            if (isBlue)
            {
                SpawnColorEnemies(BlueEnemyPrefab, blueRangeEnemyPrefab);
            }

            if (isGreen)
            {
                SpawnColorEnemies(GreenEnemyPrefab, greenRangeEnemyPrefab);
            }
        }

    private Transform GetRandomSpawnPoint()
    {
        if (spawnPoints.Length > 0)
        {
            // Choose a random spawn point from the array
            int randomIndex = Random.Range(0, spawnPoints.Length);
            return spawnPoints[randomIndex];
        }
        else
        {
            Debug.LogWarning("No spawn points assigned.");
            return null;
        }
    }

    private void SpawnColorEnemies(GameObject meleePrefab, GameObject rangedPrefab)
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            Melee = Random.value > 0.5f;
            if (Melee)
            {
                enemy.isMelee = true;
                Instantiate(meleePrefab, GetRandomSpawnPoint().position, GetRandomSpawnPoint().rotation);
            }
            else
            {
                enemy.isMelee = false;
                Instantiate(rangedPrefab, GetRandomSpawnPoint().position, GetRandomSpawnPoint().rotation);
            }
        }
    }
}
