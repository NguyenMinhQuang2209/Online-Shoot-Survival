using Unity.Netcode;
using UnityEngine;

public class PickupUpgradeItem : NetworkBehaviour
{
    [SerializeField] private UpgradeItemName upgradeItem;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsServer)
        {
            if (collision.gameObject.TryGetComponent<PlayerUpgrade>(out var playerUpgrade))
            {
                playerUpgrade.PlusItem(upgradeItem);
                Destroy(gameObject);
            }
        }
    }
}
