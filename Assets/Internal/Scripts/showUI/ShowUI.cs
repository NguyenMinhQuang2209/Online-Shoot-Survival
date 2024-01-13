using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class ShowUI : NetworkBehaviour
{
    public TextMeshProUGUI showTxt;
    public NetworkVariable<FixedString32Bytes> txt = new NetworkVariable<FixedString32Bytes>();
    public NetworkVariable<FixedString32Bytes> color = new NetworkVariable<FixedString32Bytes>();
    public float floatYSpeed = 3f;
    public float delayDieTime = 3f;
    private void Update()
    {
        showTxt.text = txt.Value.ToString();
        if (color.Value == "green")
        {
            showTxt.color = Color.green;
        }

        if (color.Value == "red")
        {
            showTxt.color = Color.red;
        }

        if (IsServer)
        {
            transform.position += floatYSpeed * Time.deltaTime * Vector3.up;
        }
    }
    public void ShowUIInit(string txt, Color color)
    {
        this.txt.Value = new(txt);
        if (color == Color.green)
        {
            this.color.Value = "green";
        }

        if (color == Color.red)
        {
            this.color.Value = "red";
        }
        Destroy(gameObject, delayDieTime);
    }
}
