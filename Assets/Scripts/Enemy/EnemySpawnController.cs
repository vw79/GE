using UnityEngine;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isEntered)
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
            GameObject spawnedEnemy;
            if (Melee)
            {
                spawnedEnemy = Instantiate(meleePrefab, GetRandomSpawnPoint().position, GetRandomSpawnPoint().rotation);
            }
            else
            {
                spawnedEnemy = Instantiate(rangedPrefab, GetRandomSpawnPoint().position, GetRandomSpawnPoint().rotation);
            }
            GameManager.Instance.RegisterEnemy(spawnedEnemy);
        }
    }
}