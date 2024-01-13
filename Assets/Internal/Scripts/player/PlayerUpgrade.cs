
using Unity.Netcode;
using UnityEngine;
public class PlayerUpgrade : NetworkBehaviour
{
    private NetworkVariable<int> plusHealth = new NetworkVariable<int>(0);
    private NetworkVariable<int> plusSpeed = new NetworkVariable<int>(0);
    private NetworkVariable<int> plusDamage = new NetworkVariable<int>(0);
    private NetworkVariable<int> plusBulletSpeed = new NetworkVariable<int>(0);
    private NetworkVariable<int> reduceTimeBwtAttack = new NetworkVariable<int>(0);
    private NetworkVariable<int> plusDelayDieTime = new NetworkVariable<int>(0);

    [Header("update config")]
    [SerializeField] private int plusHealthValue = 1;
    [SerializeField] private float plusSpeedValue = 1f;
    [SerializeField] private int recoverHealthValue = 10;

    private PlayerHealth playerHealth;
    private PlayerMovement playerMovement;
    public override void OnNetworkSpawn()
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerMovement = GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        if (IsOwner)
        {
            if (UpgradeController.instance != null)
            {
                UpgradeController.instance.ChangePlusItemValue(
                    plusHealth.Value,
                    plusSpeed.Value,
                    plusDamage.Value,
                    plusBulletSpeed.Value,
                    reduceTimeBwtAttack.Value,
                    plusDelayDieTime.Value
                    );
            }

            playerMovement.ChangePlusSpeed(plusSpeed.Value * plusSpeedValue);
        }
    }
    public void PlusItem(UpgradeItemName name)
    {
        switch (name)
        {
            case UpgradeItemName.RecoverHealth:
                playerHealth.RecoverHealth(recoverHealthValue);
                break;
            case UpgradeItemName.PlusHealth:
                plusHealth.Value += 1;
                playerHealth.ChangePlusHealth(plusHealth.Value * plusHealthValue);
                break;
            case UpgradeItemName.PlusSpeed:
                plusSpeed.Value += 1;
                break;
            case UpgradeItemName.PlusDamage:
                plusDamage.Value += 1;
                break;
            case UpgradeItemName.PlusBulletSpeed:
                plusBulletSpeed.Value += 1;
                break;
            case UpgradeItemName.ReduceTimeBwtAttack:
                reduceTimeBwtAttack.Value += 1;
                break;
            case UpgradeItemName.PlusDelayDieTime:
                plusDelayDieTime.Value += 1;
                break;

        }
    }
}
