using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ObjectPool chunkPool;
    [SerializeField] private TransformEvent objectSpawnedEvent;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnDistance = 10f;

    private Vector3 lastSpawnPosition;
    private void Start()
    {
        lastSpawnPosition = transform.position;
        SpawnInitialChunks();
    }
    private void Update()
    {
        if (Vector3.Distance(transform.position, lastSpawnPosition) >= spawnDistance)
        {
            SpawnNewChunk();
        }
    }
    private void SpawnInitialChunks()
    {
        for (int i = 0; i <= 4; i++)
        {
            SpawnNewChunk();
        }
    }
    private void SpawnNewChunk()
    {
        GameObject newChunk = chunkPool.GetFromPool();
        newChunk.transform.position = lastSpawnPosition + Vector3.forward * spawnDistance;
        lastSpawnPosition = newChunk.transform.position;

        objectSpawnedEvent.RaiseEvent(newChunk.transform);
    }
}
