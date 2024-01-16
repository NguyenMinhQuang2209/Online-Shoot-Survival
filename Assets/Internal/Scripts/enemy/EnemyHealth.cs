using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : Health
{
    private Animator animator;
    private Enemy enemy;

    [SerializeField] private float delayDieTime = 3f;
    public override void OnNetworkSpawn()
    {
        animator = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();

        if (IsServer)
        {
            HealthInit();
        }
    }
    public override void ObjectDie(ulong owner)
    {
        ObjectDie();
    }
    public override void ObjectDie()
    {
        if (enemy != null)
        {
            enemy.EnemyDie();
        }
        GetComponent<Collider2D>().enabled = false;
        DisableColliderClientRpc();
        animator.SetTrigger("Dead");
        Destroy(gameObject, delayDieTime);
    }
    [ClientRpc]
    public void DisableColliderClientRpc()
    {
        GetComponent<Collider2D>().enabled = false;
    }
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        animator.SetTrigger("Hit");
    }
}
