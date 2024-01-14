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
                Destroy(gameObject);
            }
        }
    }
}
