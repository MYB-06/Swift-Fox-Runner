using System.Collections.Generic;
using UnityEngine;

public class BatchMover : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private List<Transform> movableObjects;
    [SerializeField] private Vector3 moveDirection = Vector3.back;
    [SerializeField] private float moveSpeed;

    private void FixedUpdate()
    {
        Vector3 moveVector = moveDirection * moveSpeed * Time.fixedDeltaTime;
        int count = movableObjects.Count;

        for (int i = 0; i < count; i++)
        {
            if (movableObjects[i] != null)
            {
                movableObjects[i].position += moveVector;
            }
        }
    }
}
