using UnityEngine;

public class DirectlyWeapon : Weapon
{
    public override void Shoot()
    {
        Transform rootParent = transform.parent;
        if (rootParent == null)
        {
            return;
        }
        Vector3 shootPosition = shootPos != null ? shootPos.position : transform.position;


        for (int i = 0; i < GetBulletAmount(); i++)
        {
            float bulletAngle = GetBulletAngle();
            float angle = i == 0 ? 0 : (i % 2) != 0 ? i * bulletAngle : (i - 1) * -bulletAngle;
            GameObject spawnItemController = GameObject.FindGameObjectWithTag(TagController.SPAWN_ITEM_CONTROLLER_TAG);
            if (spawnItemController != null && spawnItemController.TryGetComponent<SpawnItemController>(out var spawnItem))
            {
                spawnItem.SpawnBulletServerRpc(
                    bullet.GetItemName(),
                    new[] { shootPosition.x, shootPosition.y, shootPosition.z },
                    new[] { 0f, 0f, rootParent.eulerAngles.z + angle },
                    GetDamage(),
                    GetBulletSpeed(),
                    GetDelayDieTime(),
                    0);
            }
        }
    }
}
