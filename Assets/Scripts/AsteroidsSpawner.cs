using UnityEngine;

public class AsteroidsSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject asteroidPrefab;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float MaxAsteroids = 15;

    [Header("Spawn Area")]
    [Tooltip("Drag a BoxCollider (set as Trigger) in here to define the spawn zone bounds.")]
    [SerializeField] private BoxCollider spawnArea;

    private float TimeSinceLastSpawn;
    private int CurrentAsteroidCount;

    private void Update()
    {
        TimeSinceLastSpawn += Time.deltaTime;

        if (TimeSinceLastSpawn >= spawnInterval && CurrentAsteroidCount < MaxAsteroids)
        {
            SpawnAsteroid();
            TimeSinceLastSpawn = 0f;
        }
    }
    private void SpawnAsteroid()
    {
        if (asteroidPrefab == null || spawnArea == null)
        {
            Debug.LogWarning("Asteroid prefab or spawn area is not assigned.");
            return;
        }

        Vector3 spawnPosition = GetRandomPositionInSpawnArea();
        GameObject Asteroid = Instantiate(asteroidPrefab, spawnPosition, Random.rotation);
        CurrentAsteroidCount++;

        Debug.Log("Asteroid spawned at" + spawnPosition);

        AsteroidBehaviour behaviour = Asteroid.GetComponent<AsteroidBehaviour>();
        if (behaviour != null)
        {
                       // behaviour.OnAsteroidDestroyed += HandleAsteroidDestroyed;
        }

        StartCoroutine(WatchForDestruction(Asteroid));
    }

    private Vector3 GetRandomPositionInSpawnArea()
    {
       Bounds bounds = spawnArea.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        float z = Random.Range(bounds.min.z, bounds.max.z);

        return new Vector3(x, y, z);
    }

    private System.Collections.IEnumerator WatchForDestruction(GameObject asteroid)
    {
     yield return new WaitUntil(() => asteroid == null);
        CurrentAsteroidCount--;
        Debug.Log("Asteroid destroyed. Current count: " + CurrentAsteroidCount);
    }
}
