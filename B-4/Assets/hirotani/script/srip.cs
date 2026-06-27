using UnityEngine;

public class srip : MonoBehaviour
{
    public GameObject gasPrefab;

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded) return;

        Instantiate(gasPrefab, transform.position, Quaternion.identity);
    }
}