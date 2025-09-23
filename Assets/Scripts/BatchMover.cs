using System.Collections.Generic;
using UnityEngine;

public class BatchMover : MonoBehaviour
{
    public static float TotalWorldMovement { get; private set; } = 0f;
    [Header("References")]
    [SerializeField] private TransformEvent objectSpawnedEvent;
    [SerializeField] private TransformEvent objectDespawnedEvent;
    [Header("Settings")]
    [SerializeField] private List<Transform> movableObjects;
    [SerializeField] private Vector3 moveDirection = Vector3.back;
    [SerializeField] private float moveSpeed;

    private void OnEnable()
    {
        objectSpawnedEvent.OnEventRaised.AddListener(AddMovableObject);
        objectDespawnedEvent.OnEventRaised.AddListener(RemoveMovableObject);
    }

    private void OnDisable()
    {
        objectSpawnedEvent.OnEventRaised.RemoveListener(AddMovableObject);
        objectDespawnedEvent.OnEventRaised.RemoveListener(RemoveMovableObject);
    }
    private void FixedUpdate()
    {
        Vector3 moveVector = moveDirection * moveSpeed * Time.fixedDeltaTime;

        TotalWorldMovement += moveVector.magnitude;

        int count = movableObjects.Count;
        for (int i = 0; i < count; i++)
        {
            if (movableObjects[i] != null)
            {
                movableObjects[i].position += moveVector;
            }
        }
    }
    private void AddMovableObject(Transform obj)
    {
        movableObjects.Add(obj);
    }
    private void RemoveMovableObject(Transform obj)
    {
        movableObjects.Remove(obj);
    }
}
