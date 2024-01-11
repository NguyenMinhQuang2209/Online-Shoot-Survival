using Unity.Netcode;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (weapon != null)
        {
            if (collision.gameObject.TryGetComponent<PlayerMovement>(out var playerMovement))
            {
                playerMovement.Equipment(weapon);
                DestroyObjectServerRpc();
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void DestroyObjectServerRpc()
    {
        Destroy(gameObject);
    }
}
