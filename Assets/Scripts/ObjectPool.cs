using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform poolParent;
    [Header("Pool Settings")]
    [SerializeField] private int poolSize;

    private Queue<GameObject> poolQueue;

    private void Awake()
    {
        poolQueue = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject pooledObject = Instantiate(prefab, poolParent);
            pooledObject.name = "Pooled_Cube_0" + i;
            pooledObject.SetActive(false);
            poolQueue.Enqueue(pooledObject);
        }
    }
    public GameObject GetFromPool()
    {
        if (poolQueue.Count > 0)
        {
            GameObject getObject = poolQueue.Dequeue();
            getObject.SetActive(true);
            return getObject;
        }
        return Instantiate(prefab, poolParent);
    }
    public void ReturnToPool(GameObject obj)
    {
        if (obj != null)
        {
            obj.SetActive(false);
            poolQueue.Enqueue(obj);
        }
    }
}
