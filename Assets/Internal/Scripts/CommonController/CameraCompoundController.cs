using UnityEngine;

public class CameraCompoundController : MonoBehaviour
{
    public Collider2D compoundCamera;
    public static CameraCompoundController instance;
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
