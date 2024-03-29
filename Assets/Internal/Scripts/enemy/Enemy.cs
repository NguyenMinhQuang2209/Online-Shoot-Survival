using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : NetworkBehaviour
{
    public static string PLAYER_TAG = "Player";

    [SerializeField] private float speed = 1f;
    [SerializeField] private int damage = 1;
    private NavMeshAgent agent;

    [Space(5)]
    [SerializeField] private float changeTargetTimer = 3f;
    float currentChangeTargetTimer = 0f;
    [SerializeField] private float takeDamageWaitTime = 3f;
    float currentTakeDamageWaitTime = 0f;
    Transform target = null;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            agent.updateUpAxis = false;
            agent.speed = speed;
            agent.enabled = true;
            currentChangeTargetTimer = changeTargetTimer;
            currentTakeDamageWaitTime = takeDamageWaitTime;
        }
    }
    private void Update()
    {
        if (IsServer)
        {
            currentChangeTargetTimer += Time.deltaTime;
            currentTakeDamageWaitTime += Time.deltaTime;
            if (currentChangeTargetTimer >= changeTargetTimer)
            {
                ChasePlayer();
                currentChangeTargetTimer = 0f;
            }
            if (target != null && agent != null && target.TryGetComponent<PlayerMovement>(out var playerMovement))
            {
                if (playerMovement.PlayerDie())
                {
                    target = null;
                }
                else
                {
                    transform.rotation = Quaternion.Euler(new(0f, target.position.x > transform.position.x ? 0f : 180f, 0f));
                    agent.SetDestination(target.position);
                }
            }
        }
    }
    private void ChasePlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag(PLAYER_TAG);
        List<GameObject> tempPlayer = new();
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].TryGetComponent<PlayerMovement>(out var playerMovement))
            {
                if (!playerMovement.PlayerDie())
                {
                    tempPlayer.Add(players[i]);
                }
            }
        }
        if (tempPlayer.Count > 0)
        {
            target = tempPlayer[0].transform;
            float distance = Vector2.Distance(transform.position, target.position);
            for (int i = 1; i < tempPlayer.Count; i++)
            {
                Transform player = tempPlayer[i].transform;
                float nextDistance = Vector2.Distance(transform.position, player.position);
                if (distance > nextDistance)
                {
                    target = player;
                    distance = nextDistance;
                }
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (IsServer)
        {
            if (currentTakeDamageWaitTime >= takeDamageWaitTime)
            {
                if (collision.gameObject.TryGetComponent<PlayerHealth>(out var health))
                {
                    health.TakeDamage(damage);
                    currentTakeDamageWaitTime = 0f;
                }
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsServer)
        {
            if (currentTakeDamageWaitTime >= takeDamageWaitTime)
            {
                if (collision.gameObject.TryGetComponent<PlayerHealth>(out var health))
                {
                    health.TakeDamage(damage);
                    currentTakeDamageWaitTime = 0f;
                }
            }
        }
    }
    public void EnemyDie()
    {
        if (agent != null)
        {
            agent.isStopped = true;
            agent.enabled = false;
            agent = null;
        }
    }
}
