using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerLight : NetworkBehaviour
{
    [SerializeField] private Light2D light2d;
    [SerializeField] private Transform lightSpawn;

    LightCustom currentLight = null;
    private PlayerMovement playerMovement;
    public override void OnNetworkSpawn()
    {
        light2d.gameObject.SetActive(IsOwner);
        playerMovement = GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        if (IsOwner)
        {
            if (playerMovement.PlayerDie())
            {
                light2d.pointLightInnerRadius = 5f;
                light2d.pointLightOuterRadius = 5f;
            }
        }
    }
    public void EquipmentLight(LightCustom customLight)
    {
        if (currentLight != null)
        {
            Destroy(currentLight.gameObject);
        }
        currentLight = Instantiate(customLight, lightSpawn);

        light2d.pointLightInnerRadius = currentLight.GetLightDegree() - 0.1f;
        light2d.pointLightOuterRadius = currentLight.GetLightDegree();
    }

}
