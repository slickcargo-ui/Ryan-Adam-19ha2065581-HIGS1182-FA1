using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject EnemyPrefab;
    [SerializeField] private float MaxEnemy = 1;

    [Header("Spawn Area")]
    [Tooltip("Drag a BoxCollider (set as Trigger) in here to define the spawn zone bounds.")]
    [SerializeField] private BoxCollider spawnArea;

    private int CurrentEnemyCount;

    public void EnemyDiffIncrease()
    {
        MaxEnemy = Mathf.Min(8, MaxEnemy + 1); // Increase max Enemy but cap at 8
        if (CurrentEnemyCount < MaxEnemy)
        {
            SpawnEnemy();
        }
        Debug.Log("Enemy count increased. MaxEnemy: " + MaxEnemy);
    }
    private void SpawnEnemy()
    {
        if (EnemyPrefab == null || spawnArea == null)
        {
            Debug.LogWarning("Enemy prefab or spawn area is not assigned.");
            return;
        }

        Vector3 spawnPosition = GetRandomPositionInSpawnArea();
        GameObject Enemy = Instantiate(EnemyPrefab, spawnPosition, Random.rotation);
        CurrentEnemyCount++;

        Debug.Log("Enemy spawned at" + spawnPosition);

        StartCoroutine(WatchForDestruction(Enemy));
    }

    private Vector3 GetRandomPositionInSpawnArea()
    {
        Bounds bounds = spawnArea.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        float z = Random.Range(bounds.min.z, bounds.max.z);

        return new Vector3(x, y, z);
    }

    private System.Collections.IEnumerator WatchForDestruction(GameObject Enemy)
    {
        yield return new WaitUntil(() => Enemy == null);
        CurrentEnemyCount--;
        Debug.Log("Enemy destroyed. Current count: " + CurrentEnemyCount);
    }
}