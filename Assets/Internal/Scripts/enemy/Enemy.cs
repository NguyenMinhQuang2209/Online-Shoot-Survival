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
            if (target != null)
            {
                agent.SetDestination(target.position);
            }
        }
    }
    private void ChasePlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag(PLAYER_TAG);
        if (players.Length > 0)
        {
            target = players[0].transform;
            float distance = Vector2.Distance(transform.position, target.position);
            for (int i = 1; i < players.Length; i++)
            {
                Transform player = players[i].transform;
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
                currentTakeDamageWaitTime = 0f;
                if (collision.gameObject.TryGetComponent<Health>(out var health))
                {
                    health.TakeDamage(damage);
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
        }
    }
}
