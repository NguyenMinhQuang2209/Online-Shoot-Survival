using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnItemManager : NetworkBehaviour
{
    [SerializeField] private List<ItemConfig> items = new();
    [SerializeField] private Vector2 spawnXAxis;
    [SerializeField] private Vector2 spawnYAxis;

    float currentTimeBwtSpawn = 0f;
    private void Start()
    {
        gameObject.SetActive(IsServer);
    }
    private void Update()
    {
        if (IsServer)
        {

        }
    }
}
[System.Serializable]
public class ItemConfig
{
    public ItemName itemName;
    public float timeBwtSpawn = 0f;
    public int currentIndex = 0;
    public int currentQuantity = 0;
    public int maxQuantity = 10;
}