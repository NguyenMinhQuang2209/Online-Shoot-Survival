using UnityEngine;

public class PlayerHealth : Health
{
    private PlayerMovement playerMovement;
    public override void OnNetworkSpawn()
    {
        playerMovement = GetComponent<PlayerMovement>();
        if (IsServer)
        {
            HealthInit();
        }
    }
    public override void ObjectDie()
    {
        if (playerMovement != null)
        {
            playerMovement.ObjectDie();
        }
    }
    private void Update()
    {

        if (!IsOwner)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamageTestServerRpc();
        }
    }
}
