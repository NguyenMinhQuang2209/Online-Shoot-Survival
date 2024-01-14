using TMPro;
using Unity.Netcode;
using UnityEngine;

public class SpawnPlayerController : NetworkBehaviour
{
    [SerializeField] private Vector2 spawnXAxis;
    [SerializeField] private Vector2 spawnYAxis;
    [SerializeField] private float waitSpawnTime = 0f;

    private readonly NetworkVariable<int> startGame = new(0);

    [SerializeField] private GameObject startGameTxt;

    bool isStartingGame = false;

    private void Start()
    {
        startGameTxt.SetActive(true);
        if (IsServer)
        {
            Invoke(nameof(SpawnPlayer), waitSpawnTime);
        }
    }
    private void Update()
    {
        if (isStartingGame)
        {
            return;
        }
        if (StartGame())
        {
            startGameTxt.SetActive(false);
            isStartingGame = true;
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
