using UnityEngine;

public class CommonController : MonoBehaviour
{
    public static CommonController instance;

    // tag
    public static string PLAYER_TAG = "Player";
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
