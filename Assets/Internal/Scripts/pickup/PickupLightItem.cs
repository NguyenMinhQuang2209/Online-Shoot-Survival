using Unity.Netcode;
using UnityEngine;

public class PickupLightItem : NetworkBehaviour
{
    public LightCustom lightCustom;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsServer)
        {
            if (collision.TryGetComponent<PlayerLight>(out var playerLight))
            {
                playerLight.EquipmentLight(lightCustom);
                if (playerLight.TryGetComponent<NetworkObject>(out var networkObject))
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
                    if (player.TryGetComponent<PlayerLight>(out var playerLight))
                    {
                        playerLight.EquipmentLight(lightCustom);
                    }
                    return;
                }
            }
        }
    }
}
