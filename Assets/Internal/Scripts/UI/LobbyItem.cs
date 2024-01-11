using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyItem : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI lobbyNameTxt;
    public TextMeshProUGUI lobbyOwnerTxt;
    public TextMeshProUGUI playerQuantityTxt;
    public Button joinLobbyBtn;
    private string lobbyId;
    private void Start()
    {
        joinLobbyBtn.onClick.AddListener(() =>
        {
            JoinLobby();
        });
    }
    public void LobbyInit(string lobbyId, string lobbyName, string lobbyOwner, string playerQuantity)
    {
        this.lobbyId = lobbyId;
        lobbyNameTxt.text = lobbyName;
        lobbyOwnerTxt.text = "Chủ lobby:" + lobbyOwner;
        playerQuantityTxt.text = playerQuantity;
    }
    private void JoinLobby()
    {
        LobbyController.instance.JoinLobbyById(lobbyId);
    }
}
