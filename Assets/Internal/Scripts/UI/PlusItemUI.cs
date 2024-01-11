using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlusItemUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI txt;
    public void PlusItemInit(Sprite icon, string txt)
    {
        this.icon.sprite = icon;
        this.txt.text = txt;
    }
}
