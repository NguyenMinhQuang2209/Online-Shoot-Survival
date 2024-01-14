using UnityEngine;

public class LightCustom : MonoBehaviour
{
    [SerializeField] private float lightDegree = 1f;
    public float GetLightDegree()
    {
        return lightDegree;
    }
}
