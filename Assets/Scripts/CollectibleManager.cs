using UnityEngine;
using System.Collections;
public class CollectibleManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject collectiblePrefab;
    [SerializeField] private int totalItemsToSpawn = 10;

    [Header("Spawn Area")]
    [Tooltip("Drag a BoxCollider (set as Trigger) in here to define the spawn zone bounds.")]
    [SerializeField] private BoxCollider spawnArea;

    private int remainingItems;

    // Public so GameManager can check progress toward the win condition.
    public int RemainingItems => remainingItems;

    private void Start()
    {
        SpawnAllItems();
    }

    // Spawns every collectible up front at random positions.
    // (Unlike asteroids, items don't need to trickle in over time.)
    private void SpawnAllItems()
    {
        if (collectiblePrefab == null || spawnArea == null)
        {
            Debug.LogWarning("CollectibleManager is missing a prefab or spawn area reference.");
            return;
        }

        for (int i = 0; i < totalItemsToSpawn; i++)
        {
            SpawnSingleItem();
        }
    }

    private void SpawnSingleItem()
    {
        Vector3 spawnPosition = GetRandomPointInArea();
        GameObject item = Instantiate(collectiblePrefab, spawnPosition, Quaternion.identity);
        remainingItems++;

        Debug.Log("Collectible spawned at " + spawnPosition);

        StartCoroutine(WatchForPickup(item));
    }

    // Same reusable pattern as AsteroidsSpawner — picks a random point inside a BoxCollider.
    private Vector3 GetRandomPointInArea()
    {
        Bounds bounds = spawnArea.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        float z = Random.Range(bounds.min.z, bounds.max.z);

        return new Vector3(x, y, z);
    }

    // Waits until the item is picked up (destroyed), then updates the remaining count
    // and tells GameManager to check whether the player has now won.
    private IEnumerator WatchForPickup(GameObject item)
    {
        yield return new WaitUntil(() => item == null);
        remainingItems--;

        if (remainingItems <= 0)
        {
            Debug.Log("All collectibles gathered.");
            GameManager.Instance?.CheckWinCondition();
        }
    }
}
