using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnItemManager : NetworkBehaviour
{
    [SerializeField] private Vector2 spawnXAxis;
    [SerializeField] private Vector2 spawnYAxis;
    [SerializeField] private float waitSpawnTimer = 3f;
    [SerializeField] private List<ItemConfig> items = new();
    float currentWaitSpawnTime = 0f;

    float currentTimeBwtSpawn = 0f;

    bool canSpawn = false;
    bool wasInit = false;
    private void Start()
    {
        gameObject.SetActive(IsServer);
    }
    private void Update()
    {
        if (IsServer)
        {
            if (!canSpawn)
            {
                currentWaitSpawnTime += Time.deltaTime;
                if (currentWaitSpawnTime >= waitSpawnTimer)
                {
                    canSpawn = true;
                }
                return;
            }

            if (!wasInit)
            {
                SpawnInit();
                wasInit = true;
                return;
            }

            currentTimeBwtSpawn += Time.deltaTime;
            for (int i = 0; i < items.Count; i++)
            {
                ItemConfig item = items[i];
                if (item.timeBwtSpawn * item.currentIndex <= currentTimeBwtSpawn)
                {
                    item.PlusCurrentIndex();
                    SpawnItem(item.item);
                }
            }
        }
    }
    private void SpawnInit()
    {
        for (int i = 0; i < items.Count; i++)
        {
            ItemConfig item = items[i];
            SpawnItem(item.item, item.maxQuantity);
        }
    }
    private void SpawnItem(GameObject item, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            SpawnItem(item);
        }
    }
    private void SpawnItem(GameObject item)
    {
        float ranX = Random.Range(Mathf.Min(spawnXAxis.x, spawnXAxis.y), Mathf.Max(spawnXAxis.x, spawnXAxis.y));
        float ranY = Random.Range(Mathf.Min(spawnYAxis.x, spawnYAxis.y), Mathf.Max(spawnYAxis.x, spawnYAxis.y));
        GameObject tempItem = Instantiate(item, new(ranX, ranY, 0f), Quaternion.identity);
        if (tempItem.TryGetComponent<NetworkObject>(out var networkObject))
        {
            networkObject.Spawn();
        }
    }
}
[System.Serializable]
public class ItemConfig
{
    public GameObject item;
    public float timeBwtSpawn = 0f;
    public int currentIndex = 1;
    public int maxQuantity = 10;
    public void PlusCurrentIndex()
    {
        currentIndex += 1;
    }
}