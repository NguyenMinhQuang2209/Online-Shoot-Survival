using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnEnemyController : NetworkBehaviour
{
    [SerializeField] private List<EnemyItem> enemies = new();

    [SerializeField] private List<SpawnEnemyConfigItem> spawns = new();
    [SerializeField] private Vector2 spawnXAxis;
    [SerializeField] private Vector2 spawnYAxis;

    private readonly List<EnemySpawnItem> tempSpawn = new();
    private readonly List<SpawnEnemyConfigItem> waitSpawnEnemy = new();
    int currentDay = -1;

    float currentTimeBwtSpawn = 0f;


    private void Start()
    {
        gameObject.SetActive(IsServer);
    }
    private void Update()
    {
        if (IsServer)
        {
            currentTimeBwtSpawn += Time.deltaTime;

            DayNightController dayNightController = PreferenceController.instance.dayNightController;

            float currentHour = dayNightController.GetCurrentHour();
            int currentWorldDay = dayNightController.GetCurrentDay();

            if (currentDay != currentWorldDay)
            {
                currentDay = currentWorldDay;
                currentTimeBwtSpawn = 0f;
                tempSpawn?.Clear();
                waitSpawnEnemy?.Clear();

                for (int i = 0; i < spawns.Count; i++)
                {
                    SpawnEnemyConfigItem temp = spawns[i];
                    if (temp.startSpawnDate >= currentDay && (temp.endSpawnDate <= currentDay || temp.endSpawnDate <= 0))
                    {
                        if (temp.spawnAtHour >= currentHour || temp.spawnAtHour <= 0f)
                        {
                            tempSpawn.Add(new(GetEnemy(temp.enemyName), temp.timeBwtSpawn));
                        }
                        else
                        {
                            waitSpawnEnemy.Add(temp);
                        }
                    }
                }

            }
            else
            {
                if (waitSpawnEnemy != null && waitSpawnEnemy.Count > 0)
                {
                    for (int i = 0; i < waitSpawnEnemy.Count; i++)
                    {
                        SpawnEnemyConfigItem temp = spawns[i];
                        if (temp.startSpawnDate >= currentDay && temp.endSpawnDate <= currentDay)
                        {
                            if (temp.spawnAtHour >= currentHour)
                            {
                                int newIndex = (int)Mathf.Ceil(currentTimeBwtSpawn / temp.timeBwtSpawn);
                                tempSpawn.Add(new(GetEnemy(temp.enemyName), temp.timeBwtSpawn, newIndex));
                                waitSpawnEnemy.RemoveAt(i);
                                i--;
                            }
                        }
                    }
                }

                if (tempSpawn != null && tempSpawn.Count > 0)
                {
                    for (int i = 0; i < tempSpawn.Count; i++)
                    {
                        EnemySpawnItem temp = tempSpawn[i];
                        if (temp.currentIndex * temp.timeBwtSpawn <= currentTimeBwtSpawn)
                        {
                            temp.PlusIndex();
                            SpawnEnemy(temp.enemy);
                        }
                    }
                }

            }
        }
    }
    private Enemy GetEnemy(EnemyName name)
    {
        foreach (EnemyItem item in enemies)
        {
            if (item.name == name)
            {
                return item.enemy;
            }
        }
        return null;
    }
    private void SpawnEnemy(Enemy enemy)
    {
        float ranX = Random.Range(Mathf.Min(spawnXAxis.x, spawnXAxis.y), Mathf.Max(spawnXAxis.x, spawnXAxis.y));
        float ranY = Random.Range(Mathf.Min(spawnYAxis.x, spawnYAxis.y), Mathf.Max(spawnYAxis.x, spawnYAxis.y));
        Enemy temp = Instantiate(enemy, new(ranX, ranY, 0f), Quaternion.identity);
        if (temp.TryGetComponent<NetworkObject>(out var networkObject))
        {
            networkObject.Spawn();
        }
    }
}
[System.Serializable]
public class EnemyItem
{
    public EnemyName name;
    public Enemy enemy;
}

[System.Serializable]
public class SpawnEnemyConfigItem
{
    public float spawnAtHour = 0f;
    public int startSpawnDate = 1;
    public int endSpawnDate = 1;
    public EnemyName enemyName;
    public float timeBwtSpawn = 2f;
}
[System.Serializable]
public class EnemySpawnItem
{
    public Enemy enemy;
    public float timeBwtSpawn;
    public int currentIndex = 0;
    public EnemySpawnItem(Enemy enemy, float timeBwtSpawn)
    {
        this.enemy = enemy;
        this.timeBwtSpawn = timeBwtSpawn;
    }
    public EnemySpawnItem(Enemy enemy, float timeBwtSpawn, int newIndex)
    {
        this.enemy = enemy;
        this.timeBwtSpawn = timeBwtSpawn;
        currentIndex = newIndex;
    }
    public void PlusIndex()
    {
        currentIndex += 1;
    }
}