using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject targetPrefab;
    public Transform player;
    public float spawnInterval = 2.0f;

    [Header("生成範囲の設定")]
    public float minRadius = 2.0f;
    public float maxRadius = 10.0f;

    [Header("一度に生成する数")]
    public int spawnCount = 5; // ここで個数を指定

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            // 5個から10個の間でランダムに生成
            int randomCount = Random.Range(5, 11);
            for (int i = 0; i < randomCount; i++)
            {
                SpawnTarget();
            }
            timer = 0;
        }
    }

    void SpawnTarget()
    {
        if (player == null) return;

        // ランダムな角度と距離を計算
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float randomDistance = Random.Range(minRadius, maxRadius);

        Vector3 spawnPosition = new Vector3(
            Mathf.Cos(angle) * randomDistance,
            Mathf.Sin(angle) * randomDistance,
            0
        );

        spawnPosition += player.position;

        Instantiate(targetPrefab, spawnPosition, Quaternion.identity);
    }
}