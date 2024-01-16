using Unity.Netcode;
using UnityEngine;

public class PickupItem : NetworkBehaviour
{
    [SerializeField] private Weapon weapon;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (weapon != null && IsServer)
        {
            if (collision.gameObject.TryGetComponent<PlayerMovement>(out var playerMovement))
            {
                playerMovement.Equipment(weapon);
                if (playerMovement.TryGetComponent<NetworkObject>(out var networkObject))
                {
                    EquipmentClientRpc(networkObject.NetworkObjectId);
                }
                Destroy(gameObject);
            }
        }
    }
    [ClientRpc]
    public void EquipmentClientRpc(ulong playerId)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            if (player.TryGetComponent<NetworkObject>(out var networkObject))
            {
                if (networkObject.NetworkObjectId == playerId)
                {
                    if (player.TryGetComponent<PlayerMovement>(out var playerMovement))
                    {
                        playerMovement.Equipment(weapon);
                    }
                    return;
                }
            }
        }
    }
}
