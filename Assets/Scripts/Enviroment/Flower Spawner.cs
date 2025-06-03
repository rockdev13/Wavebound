using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerSpawner : MonoBehaviour
{
    public Collider groundObj;
    public GameObject[] flowers;
    public float flowerCapacity;

    [Header("Loading")]
    public int flowersPerFrame;

    private void Start()
    {
        if (flowers.Length == 0)
        {
            Debug.LogError("Assign the flowers");
        }

        StartCoroutine(FlowerSpawnerCoroutine());
    }

    private IEnumerator FlowerSpawnerCoroutine()
    {
        for (int i = 0; i < flowerCapacity / flowersPerFrame; i++)
        {
            yield return null;

            for (int j = 0; j < flowersPerFrame; j++)
            {
                SpawnRandomFlower();
            }
        }
    }

    private Vector3 GetRandomPosition()
    {
        Bounds bounds = groundObj.bounds;

        float x = Random.Range(bounds.min.x + groundObj.transform.position.x, bounds.max.x + groundObj.transform.position.x);
        float z = Random.Range(bounds.min.z + groundObj.transform.position.z, bounds.max.z + groundObj.transform.position.z);
        float y = 0f;

        return new Vector3(x, y, z);
    }

    private void SpawnRandomFlower()
    {
        GameObject newFlowerPrefab = flowers[Random.Range(0, flowers.Length)];
        GameObject newFlower = Instantiate(newFlowerPrefab, GetRandomPosition(), Quaternion.identity);
        newFlower.transform.parent = transform;
    }
}
