using Unity.Netcode;
using UnityEngine;

public class SpawnPlayerController : NetworkBehaviour
{
    [SerializeField] private Vector2 spawnXAxis;
    [SerializeField] private Vector2 spawnYAxis;
    [SerializeField] private float waitSpawnTime = 0f;

    private NetworkVariable<int> startGame = new NetworkVariable<int>(0);

    private void Start()
    {
        if (IsServer)
        {
            Invoke(nameof(SpawnPlayer), waitSpawnTime);
        }
    }
    public void SpawnPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if (player.TryGetComponent<PlayerMovement>(out var playerMovement))
            {
                float ranX = Random.Range(Mathf.Min(spawnXAxis.x, spawnXAxis.y), Mathf.Max(spawnXAxis.x, spawnXAxis.y));
                float ranY = Random.Range(Mathf.Min(spawnYAxis.x, spawnYAxis.y), Mathf.Max(spawnYAxis.x, spawnYAxis.y));
                playerMovement.MovementToPosition(new(ranX, ranY));
            }
        }
        startGame.Value = 1;
    }
    public bool StartGame()
    {
        return startGame.Value == 1;
    }
}
