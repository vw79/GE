using UnityEngine;
using UnityEngine.UI;

public class EnemySpawnController : MonoBehaviour
{
    public GameObject redRangeEnemyPrefab;
    public GameObject greenRangeEnemyPrefab;
    public GameObject blueRangeEnemyPrefab;
    public GameObject MeleeEnemyPrefab;
    public Material redMelee;
    public Material greenMelee;
    public Material blueMelee;
    public LayerMask LayerShit;


    public Transform[] spawnPoints;
    public Dropdown enemyTypeDropdown;
    public InputField numberOfEnemiesInput;

    private void Start()
    {
        // Assuming you have a button to trigger the spawning, you can link it to a method like this:
        // For example, you might attach this to a UI button's onClick event
        // You can also call this method from elsewhere in your code when you want to spawn enemies
    }

    public void SpawnEnemies()
    {
        // Get the selected enemy type from the dropdown
        string selectedEnemyType = enemyTypeDropdown.options[enemyTypeDropdown.value].text;

        // Get the number of enemies to spawn from the input field
        int numberOfEnemies = int.Parse(numberOfEnemiesInput.text);

        // Spawn enemies
        for (int i = 0; i < numberOfEnemies; i++)
        {
            // Choose the correct prefab based on the selected enemy type
            GameObject enemyPrefab = GetEnemyPrefab(selectedEnemyType);

            // Choose a random spawn point
            Transform spawnPoint = GetRandomSpawnPoint();

            // Instantiate the selected enemy prefab at the chosen spawn point
            Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        }
    }

    private GameObject GetEnemyPrefab(string enemyType)
    {
        switch (enemyType)
        {
            case "Red Range":
                return redRangeEnemyPrefab; 
            
            case "Red Melee":
                return redRangeEnemyPrefab;

            case "Green":
                return greenRangeEnemyPrefab;

            case "Blue":
                return blueRangeEnemyPrefab;

            default:
                Debug.LogWarning("Invalid enemy type");
                return null;
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
}
