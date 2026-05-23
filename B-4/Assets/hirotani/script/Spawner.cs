
using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    [Header("生成設定")]
    public GameObject[] targetPrefabs;
    public Transform player;

    public float spawnInterval = 2.0f;

    [Header("生成範囲")]
    public float minRadius = 2.0f;
    public float maxRadius = 10.0f;

    [Header("生成数")]
    public int minSpawnCount = 5;
    public int maxSpawnCount = 10;

    [Header("最大生成数")]
    public int maxAliveCount = 30;  //  上限

    [Header("重なり防止")]
    public float checkRadius = 1.0f; //  最低距離

    private float timer;

    // 🔥 生成物管理リスト
    private List<GameObject> spawnedObjects = new List<GameObject>();

    void Update()
    {
        // ✅ 削除されたものをリストから除去
        spawnedObjects.RemoveAll(obj => obj == null);

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            // 🔥 上限チェック
            if (spawnedObjects.Count >= maxAliveCount) return;

            int randomCount = Random.Range(minSpawnCount, maxSpawnCount + 1);

            for (int i = 0; i < randomCount; i++)
            {
                if (spawnedObjects.Count >= maxAliveCount) break;

                SpawnTarget();
            }

            timer = 0;
        }
    }

    void SpawnTarget()
    {
        if (player == null) return;

        // 🔥 最大試行回数（無限ループ防止）
        for (int attempt = 0; attempt < 20; attempt++)
        {
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            float randomDistance = Random.Range(minRadius, maxRadius);

            Vector3 spawnPosition =
                new Vector3(
                    Mathf.Cos(angle) * randomDistance,
                    Mathf.Sin(angle) * randomDistance,
                    0
                ) + player.position;

            // 🔥 重なりチェック
            Collider2D hit = Physics2D.OverlapCircle(spawnPosition, checkRadius);

            if (hit == null)
            {
                // prefab選択
                int randomIndex = Random.Range(0, targetPrefabs.Length);
                GameObject randomPrefab = targetPrefabs[randomIndex];

                GameObject obj =
                    Instantiate(randomPrefab, spawnPosition, Quaternion.identity);

                // リストに追加
                spawnedObjects.Add(obj);

                // player渡す
                OrbitTarget orbit = obj.GetComponent<OrbitTarget>();
                if (orbit != null)
                {
                    orbit.player = player;
                }

                return; // 成功したら終了
            }
        }
    }

    // 🔵 可視化（エディタ用）
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(
            player != null ? player.position : transform.position,
            maxRadius
        );

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(
            player != null ? player.position : transform.position,
            minRadius
        );
    }
}
