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
    public virtual void Shoot()
    {

    }
    public int GetDamage()
    {
        return damage;
    }
    public float GetTimeBwtAttack()
    {
        return timeBwtAttack;
    }
    public float GetDelayDieTime()
    {
        return delayDieTime;
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
        return bulletSpeed;
    }
}
