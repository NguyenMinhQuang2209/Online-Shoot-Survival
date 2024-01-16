using Cinemachine;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDeadController : NetworkBehaviour
{
    public GameObject playerDeadUI;
    public TextMeshProUGUI playerNameTxt;
    public Button outBtn;
    private List<GameObject> playerList = new();
    int currentIndex = 0;

    bool wasInit = false;
    GameObject currentObject = null;
    GameObject nextObject = null;
    private void Start()
    {
        playerDeadUI.SetActive(false);
    }
    public void PlayerDead()
    {
        playerDeadUI.SetActive(true);
        if (!wasInit)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players)
            {
                playerList.Add(player);
            }
            wasInit = true;
            Next();
        }
    }
    public void Next()
    {
        currentObject = playerList[currentIndex];
        currentIndex = currentIndex <= 0 ? playerList.Count - 1 : currentIndex - 1;
        nextObject = playerList[currentIndex];

        ReloadUserName();
    }
    public void Previous()
    {
        currentObject = playerList[currentIndex];
        currentIndex = currentIndex >= playerList.Count - 1 ? 0 : currentIndex + 1;
        nextObject = playerList[currentIndex];

        ReloadUserName();
    }
    private void ReloadUserName()
    {
        if (currentObject.TryGetComponent<PlayerMovement>(out var currentPlayerMovement))
        {
            CinemachineVirtualCamera mainCamera = currentPlayerMovement.GetMainCamera();
            mainCamera.Priority = 0;
            mainCamera.enabled = false;
            currentPlayerMovement.ChangeLightStatus(false);
        }
        if (nextObject.TryGetComponent<PlayerMovement>(out var playerMovement))
        {
            playerNameTxt.text = playerMovement.GetUserName();
            CinemachineVirtualCamera mainCamera = playerMovement.GetMainCamera();
            mainCamera.Priority = 1;
            mainCamera.enabled = true;
            currentPlayerMovement.ChangeLightStatus(true);
        }
    }
    public void OutMatch()
    {
        if (!IsServer)
        {
            NetworkManager.Singleton.Shutdown();
            if (SceneController.instance != null)
            {
                SceneController.instance.ChangeScene(SceneController.SceneName.Lobby, true);
            }
        }
        else
        {
            bool canEnd = true;
            for (int i = 0; i < playerList.Count; i++)
            {
                if (playerList[i].TryGetComponent<PlayerMovement>(out var playerMovement))
                {
                    if (!playerMovement.PlayerDie())
                    {
                        canEnd = false;
                        break;
                    }
                }
            }
            if (!canEnd)
            {
                ShowDeadTxt showDeadTxt = PreferenceController.instance.showDeadTxtController;
                showDeadTxt.ShowLog("Người chơi chưa chết hết!");
            }
            else
            {
                SceneController.instance.ChangeSceneSync(SceneController.SceneName.SelectScene, true);
            }
        }
    }
}
