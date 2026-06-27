using UnityEngine;

public class GasZoneVisual : MonoBehaviour
{
    public float radius = 4f;

    void Start()
    {
        transform.localScale = new Vector3(radius * 2, radius * 2, 1);
    }
}
