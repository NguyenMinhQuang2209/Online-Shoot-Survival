using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeController : MonoBehaviour
{
    public static UpgradeController instance;

    private int plusHealth = 0;
    private int plusSpeed = 0;
    private int plusDamage = 0;
    private int plusBulletSpeed = 0;
    private int reduceTimeBwtAttack = 0;
    private int plusDelayDieTime = 0;

    [Header("Plus config")]
    [SerializeField] private int plusDamageValue = 1;
    [SerializeField] private float plusBulletSpeedValue = 1f;
    [SerializeField] private float reduceTimeBwtAttackValue = 1f;
    [SerializeField] private float plusDelayDieTimeValue = 1f;

    [Space(5)]
    [Header("Plus UI COnfig")]
    [SerializeField] private Transform plusUIContainer;
    [SerializeField] private PlusItemUI plusItemUI;
    [SerializeField] private Sprite plusHealthUI;
    [SerializeField] private Sprite plusSpeedUI;
    [SerializeField] private Sprite plusDamageUI;
    [SerializeField] private Sprite plusBulletSpeedUI;
    [SerializeField] private Sprite reduceTimeBwtAttackUI;
    [SerializeField] private Sprite plusDelayDieTimeUI;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    public void ChangePlusItemValue(
        int plusHealth,
        int plusSpeed,
        int plusDamage,
        int plusBulletSpeed,
        int reduceTimeBwtAttack,
        int plusDelayDieTime)
    {
        bool needReloadUI = false;
        if (this.plusHealth != plusHealth)
        {
            this.plusHealth = plusHealth;
            needReloadUI = true;
        }

        if (this.plusSpeed != plusSpeed)
        {
            this.plusSpeed = plusSpeed;
            needReloadUI = true;
        }

        if (this.plusDamage != plusDamage)
        {
            this.plusDamage = plusDamage;
            needReloadUI = true;
        }
        if (this.plusBulletSpeed != plusBulletSpeed)
        {
            this.plusBulletSpeed = plusBulletSpeed;
            needReloadUI = true;
        }
        if (this.reduceTimeBwtAttack != reduceTimeBwtAttack)
        {
            this.reduceTimeBwtAttack = reduceTimeBwtAttack;
            needReloadUI = true;
        }
        if (this.plusDelayDieTime != plusDelayDieTime)
        {
            this.plusDelayDieTime = plusDelayDieTime;
            needReloadUI = true;
        }

        if (needReloadUI)
        {
            ReloadUI();
        }
    }
    public void ReloadUI()
    {
        foreach (Transform child in plusUIContainer)
        {
            Destroy(child.gameObject);
        }

        CheckAndLoadUI(plusHealth, plusHealthUI);
        CheckAndLoadUI(plusSpeed, plusSpeedUI);
        CheckAndLoadUI(plusDamage, plusDamageUI);
        CheckAndLoadUI(plusBulletSpeed, plusBulletSpeedUI);
        CheckAndLoadUI(reduceTimeBwtAttack, reduceTimeBwtAttackUI);
        CheckAndLoadUI(plusDelayDieTime, plusDelayDieTimeUI);

    }
    private void CheckAndLoadUI(int plus, Sprite icon)
    {
        if (plus > 0)
        {
            PlusItemUI temp = Instantiate(plusItemUI, plusUIContainer);
            temp.PlusItemInit(icon, plus.ToString());
        }
    }

    public int GetPlusDamage()
    {
        return plusDamage * plusDamageValue;
    }
    public float GetPlusBulletSpeed()
    {
        return plusBulletSpeed * plusBulletSpeedValue;
    }
    public float GetReduceTimeBwtAttack()
    {
        return reduceTimeBwtAttack * reduceTimeBwtAttackValue;
    }
    public float GetPlusDelayDieTime()
    {
        return plusDelayDieTime * plusDelayDieTimeValue;
    }
}
