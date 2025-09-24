using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ObjectPool chunkPool;
    [SerializeField] private LayeredTransformEvent objectSpawnedEvent;
    [SerializeField] private LayeredTransformEvent objectDespawnedEvent;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnDistance = 10f;

    private Vector3 lastSpawnPosition;
    private List<GameObject> activeChunks = new List<GameObject>();
    private float lastSpawnTrigger = 0f;
    private void Start()
    {
        lastSpawnPosition = transform.position;
        SpawnInitialChunks();
    }
    private void Update()
    {
        if (BatchMover.TotalWorldMovement - lastSpawnTrigger >= spawnDistance)
        {
            SpawnNewChunk();
            lastSpawnTrigger = BatchMover.TotalWorldMovement;
        }
        CheckForDespawn();
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

        if (newChunk == null) return;

        newChunk.transform.position = lastSpawnPosition + Vector3.forward * spawnDistance;
        lastSpawnPosition = newChunk.transform.position;

        activeChunks.Add(newChunk);
        objectSpawnedEvent.RaiseEvent(newChunk.transform, LayerType.Foreground);
    }
    private void CheckForDespawn()
    {
        for (int i = activeChunks.Count - 1; i >= 0; i--)
        {
            if (activeChunks[i].transform.position.z < -spawnDistance)
            {
                DespawnChunk(activeChunks[i]);
            }
        }
    }
    private void DespawnChunk(GameObject chunk)
    {
        objectDespawnedEvent.RaiseEvent(chunk.transform, LayerType.Foreground);
        activeChunks.Remove(chunk);
        chunkPool.ReturnToPool(chunk);
    }
}
