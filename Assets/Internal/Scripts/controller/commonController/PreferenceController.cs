using UnityEngine;

public class PreferenceController : MonoBehaviour
{
    public static PreferenceController instance;
    public SpawnItemController spawnItemController;
    public DayNightController dayNightController;
    public SpawnPlayerController spawnPlayerController;
    public ShowDeadTxt showDeadTxtController;
    public PlayerDeadController playerDeadController;
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
