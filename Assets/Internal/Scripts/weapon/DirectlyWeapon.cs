using UnityEngine;

public class DirectlyWeapon : Weapon
{
    public override void Shoot(ulong owner)
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
            if (PreferenceController.instance != null)
            {
                PreferenceController.instance.spawnItemController.SpawnBulletServerRpc(
                    bullet.GetItemName(),
                    new[] { shootPosition.x, shootPosition.y, shootPosition.z },
                    new[] { 0f, 0f, rootParent.eulerAngles.z + angle },
                    GetDamage(),
                    GetBulletSpeed(),
                    GetDelayDieTime(),
                    0,
                    owner);
            }
        }
    }
}
