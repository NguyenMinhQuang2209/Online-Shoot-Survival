using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class TempController : MonoBehaviour
{
    public Button startHost;
    public Button startClient;
    private void Start()
    {
        startHost.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
        });

        startClient.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });
    }
}
