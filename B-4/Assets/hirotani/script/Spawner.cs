using UnityEngine;

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

    [Header("回転設定")]
    public Transform target;
    public float speed = 100f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            int randomCount =
                Random.Range(
                    minSpawnCount,
                    maxSpawnCount + 1);

            for (int i = 0; i < randomCount; i++)
            {
                SpawnTarget();
            }

            timer = 0;
        }

        // 回転
        if (target != null &&
            player != null)
        {
            target.RotateAround(
                player.position,
                Vector3.forward,
                speed * Time.deltaTime
            );
        }
    }

    void SpawnTarget()
    {
        if (player == null) return;

        float angle =
            Random.Range(0f, 360f)
            * Mathf.Deg2Rad;

        float randomDistance =
            Random.Range(
                minRadius,
                maxRadius);

        Vector3 spawnPosition =
            new Vector3(
                Mathf.Cos(angle) * randomDistance,
                Mathf.Sin(angle) * randomDistance,
                0
            );

        spawnPosition += player.position;

        // ランダムPrefab選択
        int randomIndex =
            Random.Range(
                0,
                targetPrefabs.Length);

        GameObject randomPrefab =
            targetPrefabs[randomIndex];

        GameObject obj =
            Instantiate(
                randomPrefab,
                spawnPosition,
                Quaternion.identity
            );

        // player渡す
        OrbitTarget orbit =
            obj.GetComponent<OrbitTarget>();

        if (orbit != null)
        {
            orbit.player = player;
        }
    }
}