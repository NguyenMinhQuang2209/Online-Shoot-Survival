using UnityEngine;

public class PreferenceController : MonoBehaviour
{
    public static PreferenceController instance;
    public SpawnItemController spawnItemController;
    public DayNightController dayNightController;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    /*private void Update()
    {
        if (spawnItemController == null)
        {
            GameObject spawnItemObject = GameObject.FindGameObjectWithTag(TagController.SPAWN_ITEM_CONTROLLER_TAG);
            if (spawnItemObject != null && spawnItemObject.TryGetComponent<SpawnItemController>(out spawnItemController))
            {

            }
        }
    }*/
}
