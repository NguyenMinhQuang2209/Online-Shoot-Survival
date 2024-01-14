using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ShowDeadTxt : NetworkBehaviour
{
    public TextMeshProUGUI deadTxt;
    public Transform deadTxtContainer;
    public float delayDieTimer = 3f;

    [ServerRpc(RequireOwnership = false)]
    public void ShowDeadTxtServerRpc(string txt)
    {
        ShowDeadTxtClientRpc(txt);
    }

    [ClientRpc]
    public void ShowDeadTxtClientRpc(string txt)
    {
        TextMeshProUGUI tempTxt = Instantiate(deadTxt, deadTxtContainer);
        tempTxt.text = txt;
        Destroy(tempTxt, delayDieTimer);
    }

    public void ShowLog(string txt)
    {
        TextMeshProUGUI tempTxt = Instantiate(deadTxt, deadTxtContainer);
        tempTxt.text = txt;
        tempTxt.color = Color.red;
        Destroy(tempTxt, delayDieTimer);
    }
}
