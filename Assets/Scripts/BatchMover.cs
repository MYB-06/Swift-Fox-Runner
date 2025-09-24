using System.Collections.Generic;
using UnityEngine;

public class BatchMover : MonoBehaviour
{
    public static float TotalWorldMovement { get; private set; } = 0f;

    [Header("References")]
    [SerializeField] private LayeredTransformEvent objectSpawnedEvent;
    [SerializeField] private LayeredTransformEvent objectDespawnedEvent;
    [Header("Settings")]
    [SerializeField] private Vector3 moveDirection = Vector3.back;
    [SerializeField] private float moveSpeed;
    [Header("Layer Settings")]
    [SerializeField] private float backgroundSpeed = 0.3f;
    [SerializeField] private float foregroundSpeed = 1f;
    [SerializeField] private Transform[] manualBackgroundObjects;
    [SerializeField] private Transform[] manualForegroundObjects;

    private Dictionary<LayerType, List<Transform>> layeredObjects;
    private Dictionary<LayerType, float> layerSpeedMultipliers;

    private void Awake()
    {
        InitializeLayerSystem();
    }
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
        foreach (var layerPair in layeredObjects)
        {
            LayerType layer = layerPair.Key;
            List<Transform> objects = layerPair.Value;
            float speedMultiplier = layerSpeedMultipliers[layer];

            Vector3 layerMoveVector = moveDirection * (moveSpeed * speedMultiplier);
            TotalWorldMovement += layerMoveVector.magnitude;

            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i] != null)
                {
                    objects[i].position += layerMoveVector;
                }
            }
        }
    }
    private void AddMovableObject(Transform obj, LayerType layer)
    {
        layeredObjects[layer].Add(obj);
    }
    private void RemoveMovableObject(Transform obj, LayerType layer)
    {
        layeredObjects[layer].Remove(obj);
    }
    private void InitializeLayerSystem()
    {
        layeredObjects = new Dictionary<LayerType, List<Transform>>
        {
            {LayerType.Background, new List<Transform>()},
            {LayerType.Foreground, new List<Transform>()}
        };
        layerSpeedMultipliers = new Dictionary<LayerType, float>
        {
            {LayerType.Background, backgroundSpeed},
            {LayerType.Foreground, foregroundSpeed}
        };

        foreach (Transform obj in manualBackgroundObjects)
        {
            if (obj != null) layeredObjects[LayerType.Background].Add(obj);
        }
        foreach (Transform obj in manualForegroundObjects)
        {
            if (obj != null) layeredObjects[LayerType.Foreground].Add(obj);
        }
    }
}
