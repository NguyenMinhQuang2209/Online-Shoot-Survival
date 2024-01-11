using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class Health : NetworkBehaviour
{
    [SerializeField] private NetworkVariable<int> maxHealth = new NetworkVariable<int>(100);
    protected NetworkVariable<int> plusHealth = new NetworkVariable<int>(0);
    [SerializeField] private NetworkVariable<int> currentHealth = new NetworkVariable<int>(0);
    public void HealthInit()
    {
        currentHealth.Value = GetMaxHealth();
    }
    public int GetMaxHealth()
    {
        return maxHealth.Value + plusHealth.Value;
    }
    public virtual void TakeDamage(int damage)
    {
        currentHealth.Value = Mathf.Max(0, currentHealth.Value - damage);
        if (currentHealth.Value == 0)
        {
            ObjectDie();
        }
    }
    public virtual void ObjectDie()
    {

    }


    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageTestServerRpc()
    {
        TakeDamage(120);
        LogController.instance.Log("Take Damage: " + 120);
    }
    public int GetCurrentHealth()
    {
        return currentHealth.Value;
    }
}
