using UnityEngine;

public class PlanetRotation : MonoBehaviour
{
    [SerializeField]
    private float speed = 20f;

    void Update()
    {
        transform.Rotate(0f, 0f, speed * Time.deltaTime);
    }
}