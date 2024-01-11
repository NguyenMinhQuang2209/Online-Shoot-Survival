using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InsideLobbyItem : MonoBehaviour
{
    public TextMeshProUGUI nameTxt;
    public TextMeshProUGUI roleTxt;
    public GameObject checkedObject;
    public Button kickedBtn;
    public TextMeshProUGUI kickedTxt;
    private string playerId = "";
    private void Start()
    {
        kickedBtn.onClick.AddListener(() =>
        {
            OnKickPlayer();
        });
    }
    private void OnKickPlayer()
    {
        if (LobbyController.instance.IsLobbyOwner())
        {
            LobbyController.instance.OnOutLobby(playerId);
        }
    }
    public void InsideLobbyItemInit(string playerId, string name, bool isHost, bool check = false)
    {
        this.playerId = playerId;
        nameTxt.text = name;
        roleTxt.text = isHost ? "Chủ phòng" : "Khách";
        kickedTxt.text = isHost ? "Kick" : "Hi";
        checkedObject.SetActive(check);
    }
}
