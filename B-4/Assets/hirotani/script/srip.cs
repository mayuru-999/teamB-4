using System.Collections;
using UnityEngine;

public class srip : MonoBehaviour
{
    public int damage = 10; // ←これを追加

    public GameObject gasPrefab;
    public float radius = 5f;

    private bool isQuitting = false;

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded) return;

        Vector3 pos = transform.position;

        // 1フレーム遅らせて生成
        StartCoroutine(SpawnGas(pos));
    }

    IEnumerator SpawnGas(Vector3 pos)
    {
        yield return null; // ←ここが重要（破壊後に回す）

        Instantiate(gasPrefab, pos, Quaternion.identity);
    }
    void ApplyDamage()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider hit in hits)
        {
            // ★ Targetタグ以外は無視
            if (!hit.CompareTag("Target")) continue;

            HPmanager hp = hit.GetComponent<HPmanager>();

            if (hp != null)
            {
                hp.TakeDamage(damage);
            }
        }
    }
}