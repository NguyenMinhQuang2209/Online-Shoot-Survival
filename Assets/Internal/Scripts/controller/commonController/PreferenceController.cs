using UnityEngine;

public class PreferenceController : MonoBehaviour
{
    public static PreferenceController instance;
    [HideInInspector] public SpawnItemController spawnItemController;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    private void Update()
    {
        if (spawnItemController == null)
        {
            GameObject spawnItemObject = GameObject.FindGameObjectWithTag(TagController.SPAWN_ITEM_CONTROLLER_TAG);
            if (spawnItemObject != null && spawnItemObject.TryGetComponent<SpawnItemController>(out spawnItemController))
            {

            }
        }
    }
}
