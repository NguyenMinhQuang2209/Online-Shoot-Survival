﻿using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Health
{
    private PlayerMovement playerMovement;
    [SerializeField] private float maxMana = 100f;
    [SerializeField] private float useManaRate = 1f;
    [SerializeField] private float waitRecoverMana = 5f;
    [SerializeField] private float recoverManaRate = 1f;
    [SerializeField] private float waitRecoverManaRate = 1f;
    float currentWaitRecoverMana = 0f;
    private float currentMana = 0f;


    private Slider healthSlider;
    private Slider manaSlider;
    private TextMeshProUGUI healthTxt;
    private TextMeshProUGUI manaTxt;

    bool canUseMana = true;

    public override void OnNetworkSpawn()
    {
        playerMovement = GetComponent<PlayerMovement>();
        currentMana = maxMana;
        if (IsServer)
        {
            HealthInit();
        }
    }
    public override void ObjectDie()
    {
        if (playerMovement != null)
        {
            playerMovement.ObjectDie();
            string shooterName = "Quái vật";
            string userName = playerMovement.GetUserName();
            if (PreferenceController.instance != null)
            {
                ShowDeadTxt deadTxt = PreferenceController.instance.showDeadTxtController;
                if (deadTxt != null)
                {
                    deadTxt.ShowDeadTxtServerRpc(shooterName + " đã giết " + userName);
                }
            }
        }
    }
    public override void ObjectDie(ulong owner)
    {
        if (playerMovement != null)
        {
            playerMovement.ObjectDie();
        }
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject tempPlayer = null;
        foreach (GameObject player in players)
        {
            if (player != null && player.TryGetComponent<NetworkObject>(out var networkObjectId))
            {
                if (networkObjectId.NetworkObjectId == owner)
                {
                    tempPlayer = player;
                    break;
                }
            }
        }
        string shooterName = "Quái vật";
        string userName = playerMovement.GetUserName();
        if (tempPlayer != null && tempPlayer.TryGetComponent<PlayerMovement>(out var targetMovement))
        {
            shooterName = targetMovement.GetUserName();
        }
        if (PreferenceController.instance != null)
        {
            ShowDeadTxt deadTxt = PreferenceController.instance.showDeadTxtController;
            if (deadTxt != null)
            {
                deadTxt.ShowDeadTxtServerRpc(shooterName + " đã giết " + userName);
            }
        }
    }

    private void Update()
    {

        if (!IsOwner)
        {
            return;
        }
        if (healthSlider != null)
        {
            healthSlider.value = GetCurrentHealth();
            healthSlider.maxValue = GetMaxHealth();
            if (healthTxt != null)
            {
                healthTxt.text = GetCurrentHealth() + "/" + GetMaxHealth();
            }
        }
        if (manaSlider != null)
        {
            manaSlider.value = currentMana;
            if (manaTxt != null)
            {
                manaTxt.text = (int)currentMana + "/" + (int)maxMana;
            }
        }

        RecoverManaTime();
    }
    private void RecoverManaTime()
    {
        currentWaitRecoverMana += Time.deltaTime * waitRecoverManaRate;
        if (currentWaitRecoverMana >= waitRecoverMana)
        {
            currentMana = Mathf.Min(maxMana, currentMana + Time.deltaTime * recoverManaRate);
            canUseMana = true;
        }
    }
    public bool UseMana()
    {
        currentWaitRecoverMana = 0f;

        if (!canUseMana)
        {
            return false;
        }
        currentMana = Mathf.Max(0f, currentMana - Time.deltaTime * useManaRate);
        if (currentMana == 0)
        {
            canUseMana = false;
        }
        return true;
    }
    public void HealthSliderConfig(Slider healthSlider, Slider manaSlider,
        TextMeshProUGUI healthTxt, TextMeshProUGUI manaTxt)
    {
        this.healthSlider = healthSlider;
        this.manaSlider = manaSlider;
        this.healthTxt = healthTxt;
        this.manaTxt = manaTxt;

        SliderInit();
    }
    private void SliderInit()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = GetMaxHealth();
            healthSlider.minValue = 0f;
            healthSlider.value = GetCurrentHealth();
        }

        if (manaSlider != null)
        {
            manaSlider.maxValue = maxMana;
            manaSlider.minValue = 0f;
            manaSlider.value = currentMana;
        }
    }

    public void ChangePlusHealth(int v)
    {
        plusHealth.Value = Mathf.Max(v, 0);
        if (healthSlider != null)
        {
            healthSlider.maxValue = GetMaxHealth();
        }
    }
}
