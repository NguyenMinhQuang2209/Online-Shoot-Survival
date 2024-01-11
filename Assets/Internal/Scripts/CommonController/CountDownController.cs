using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class CountDownController : NetworkBehaviour
{
    [SerializeField] private NetworkVariable<float> countDown = new NetworkVariable<float>(20);

    public TextMeshProUGUI countDownTxt;
    private void Update()
    {
        countDownTxt.text = "Trận chiến sẽ bắt đầu sau " + (int)countDown.Value + "s";
        if (IsServer)
        {
            countDown.Value = Mathf.Max(0f, countDown.Value - Time.deltaTime);
            if (countDown.Value == 0f)
            {
                StartGame();
            }
        }
    }
    private void StartGame()
    {
        if (IsServer)
        {
            SceneController.instance.ChangeSceneSync(SceneController.SceneName.Map_1, true);
        }
    }
}
