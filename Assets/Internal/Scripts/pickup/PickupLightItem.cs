using Unity.Netcode;
using UnityEngine;

public class PickupLightItem : NetworkBehaviour
{
    public LightCustom lightCustom;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerLight>(out var playerLight))
        {
            playerLight.EquipmentLight(lightCustom);
            DestroyObjectServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void DestroyObjectServerRpc()
    {
        Destroy(gameObject);
    }
}
