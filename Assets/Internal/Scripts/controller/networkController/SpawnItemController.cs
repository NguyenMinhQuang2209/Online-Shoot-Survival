using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnItemController : MonoBehaviour
{
    public static SpawnItemController instance;
    [SerializeField] private List<PreferenceObjectItem> prefabs = new();

    [SerializeField] private List<PreferenceUpgradeObjectItem> prefabUpgradeList = new();
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnItemServerRpc(string itemName, float[] spawnPosition, float[] spawnRotation)
    {
        GameObject prefab = null;
        foreach (var item in prefabs)
        {
            if (item.name.ToString() == itemName)
            {
                prefab = item.prefab;
                break;
            }
        }
        if (prefab != null)
        {
            GameObject tempItem = Instantiate(prefab, new(spawnPosition[0], spawnPosition[1], spawnPosition[2]),
                Quaternion.Euler(new(spawnRotation[0], spawnRotation[1], spawnRotation[2])));
            if (tempItem.TryGetComponent<NetworkObject>(out var networkObject))
            {
                networkObject.Spawn();
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnUpgradeItemServerRpc(string itemName, float[] spawnPosition, float[] spawnRotation)
    {
        GameObject prefab = null;
        foreach (var item in prefabUpgradeList)
        {
            if (item.name.ToString() == itemName)
            {
                prefab = item.prefab;
                break;
            }
        }
        if (prefab != null)
        {
            GameObject tempItem = Instantiate(prefab, new(spawnPosition[0], spawnPosition[1], spawnPosition[2]),
                Quaternion.Euler(new(spawnRotation[0], spawnRotation[1], spawnRotation[2])));
            if (tempItem.TryGetComponent<NetworkObject>(out var networkObject))
            {
                networkObject.Spawn();
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnBulletServerRpc(string itemName,
        float[] spawnPosition,
        float[] spawnRotation,
        int damage,
        float speed,
        float delayDieTime,
        ulong targetId = 0)
    {
        GameObject prefab = null;
        foreach (var item in prefabs)
        {
            if (item.name.ToString() == itemName)
            {
                prefab = item.prefab;
                break;
            }
        }
        if (prefab != null)
        {
            GameObject tempItem = Instantiate(prefab, new(spawnPosition[0], spawnPosition[1], spawnPosition[2]),
                Quaternion.Euler(new(spawnRotation[0], spawnRotation[1], spawnRotation[2])));
            if (tempItem.TryGetComponent<NetworkObject>(out var networkObject) && tempItem.TryGetComponent<Bullet>(out var bullet))
            {
                networkObject.Spawn();
                bullet.BulletInit(damage, speed, delayDieTime, tempItem.transform.right * 2f, targetId);
            }

        }
    }
}
[System.Serializable]
public class PreferenceObjectItem
{
    public ItemName name;
    public GameObject prefab;
}
[System.Serializable]
public class PreferenceUpgradeObjectItem
{
    public UpgradeItemName name;
    public GameObject prefab;
}