using Unity.Netcode;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float bulletSpeed = 1f;
    [SerializeField] private float timeBwtAttack = 1f;
    [SerializeField] private float delayDieTime = 1f;
    [SerializeField] private int bulletAmount = 1;
    [SerializeField] private float bulletAngle = 8f;
    [SerializeField] protected Bullet bullet;
    [SerializeField] protected Transform shootPos;
    public virtual void Shoot(ulong owner)
    {

    }
    public int GetDamage()
    {
        int plus = 0;
        if (UpgradeController.instance != null)
        {
            plus = UpgradeController.instance.GetPlusDamage();
        }
        return damage + plus;
    }
    public float GetTimeBwtAttack()
    {
        float plus = 0;
        if (UpgradeController.instance != null)
        {
            plus = UpgradeController.instance.GetReduceTimeBwtAttack();
        }
        return timeBwtAttack - plus;
    }
    public float GetDelayDieTime()
    {
        float plus = 0;
        if (UpgradeController.instance != null)
        {
            plus = UpgradeController.instance.GetPlusDelayDieTime();
        }
        return delayDieTime + plus;
    }
    public int GetBulletAmount()
    {
        return bulletAmount;
    }
    public float GetBulletAngle()
    {
        return bulletAngle;
    }
    public float GetBulletSpeed()
    {
        float plus = 0;
        if (UpgradeController.instance != null)
        {
            plus = UpgradeController.instance.GetPlusBulletSpeed();
        }
        return bulletSpeed + plus;
    }
}
