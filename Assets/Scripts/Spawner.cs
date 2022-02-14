using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Inspector variables
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private int spawnLimit;
    [SerializeField] private int delayBetweenSpawnsInSeconds;

    // Private variables
    private int currentSpawnCount = 0;

    // Init
    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    // Spawn every n seconds
    IEnumerator SpawnEnemies()
    {
        while (currentSpawnCount < spawnLimit)
        {
            Instantiate(objectToSpawn, gameObject.transform.position, gameObject.transform.rotation);
            yield return new WaitForSeconds(delayBetweenSpawnsInSeconds);
        }
        yield return null;
    }
}
