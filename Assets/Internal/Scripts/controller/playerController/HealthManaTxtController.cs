using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthManaTxtController : MonoBehaviour
{
    public static HealthManaTxtController instance;
    public Slider healthSlider;
    public TextMeshProUGUI healthTxt;
    public Slider manaSlider;
    public TextMeshProUGUI manaTxt;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
}
