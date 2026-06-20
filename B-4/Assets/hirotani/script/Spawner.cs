using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    [Header("生成設定")]
    public GameObject[] targetPrefabs;
    public Transform player;

    [Header("生成数維持")]
    public int keepAliveCount = 20;   // 常に保つ数

    [Header("生成範囲")]
    public float minRadius = 2.0f;
    public float maxRadius = 10.0f;

    [Header("重なり防止")]
    public float checkRadius = 1.0f;
    [Header("特殊敵プレハブ")]
    public GameObject[] specialTargetPrefabs;

    [Range(0f, 1f)]
    public float specialSpawnRate = 0.1f; // 10%

    // 生成物管理リスト
    private List<GameObject> spawnedObjects = new List<GameObject>();

    void Start()
    {
        // ゲーム開始時に即20個生成
        SpawnUntilKeepCount();
    }

    void Update()
    {
        // 削除されたものをリストから除去
        spawnedObjects.RemoveAll(obj => obj == null);

        // 足りなければ即補充
        SpawnUntilKeepCount();
    }

    System.Collections.IEnumerator ScaleUp(Transform t, Vector3 target)
    {
        float time = 0f;
        float duration = 0.4f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float ratio = time / duration;

            // ポッと出る感じ（バウンド）
            float scale = Mathf.Sin(ratio * Mathf.PI * 0.5f)
                        + Mathf.Sin(ratio * Mathf.PI) * 0.15f;

            t.localScale = target * scale;

            yield return null;
        }

        t.localScale = target;
    }


    void SpawnUntilKeepCount()
    {
        int targetCount = keepAliveCount;

        if (SkillManage.Instance != null)
        {
            targetCount += Mathf.RoundToInt(
                SkillManage.Instance.getEffect(SkillEffect.Type.PlaneVolume)
            );
        }

        int failCount = 0;

        while (spawnedObjects.Count < targetCount)
        {
            int before = spawnedObjects.Count;

            SpawnTarget();

            if (spawnedObjects.Count == before)
                failCount++;
            else
                failCount = 0;

            if (failCount > 10)
                break;
        }
    }

    void SpawnTarget()
    {
        if (player == null || targetPrefabs.Length == 0) return;

        // 最大試行回数（無限ループ防止）
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

            // 重なりチェック
            Collider2D hit =
                Physics2D.OverlapCircle(spawnPosition, checkRadius);

            

            if (hit == null)
            {
                GameObject prefabToSpawn;

                // 10%で特殊敵
                if (specialTargetPrefabs.Length > 0 &&
                    Random.value < specialSpawnRate)
                {
                    int specialIndex =
                        Random.Range(0, specialTargetPrefabs.Length);

                    prefabToSpawn =
                        specialTargetPrefabs[specialIndex];
                }
                else
                {
                    int normalIndex =
                        Random.Range(0, targetPrefabs.Length);

                    prefabToSpawn =
                        targetPrefabs[normalIndex];
                }

                GameObject obj = Instantiate(
                    prefabToSpawn,
                    spawnPosition,
                    Quaternion.identity
                );

                // 一旦デフォルトの値をセット
                Vector3 targetScale = Vector3.one * 0.4f;
                Vector3 targetHealth = Vector3.one * 3f;
                Vector3 targetDastpar = Vector3.one * 5f;

                // 惑星のHPmanagerコンポーネントを取得
                HPmanager hp = obj.GetComponent<HPmanager>();

                if (SkillManage.Instance != null && hp != null)
                {
                    // 現在のレベルのデータを一括取得
                    var (health, size, dastpar) = SkillManage.Instance.LvtoPlaneData();

                    targetHealth = health;
                    targetDastpar = dastpar;

                    // 🔴 惑星自身の sizeType (1~3) に応じて、Vector3 から正しいスケールを抽出する
                    if (hp.sizeType == 1) targetScale = Vector3.one * size.x; // 大きさ1のスケール
                    else if (hp.sizeType == 2) targetScale = Vector3.one * size.y; // 大きさ2のスケール
                    else if (hp.sizeType == 3) targetScale = Vector3.one * size.z; // 大きさ3のスケール
                }

                // 初期スケールを0にしてから、各サイズに応じた targetScale へアニメーション
                obj.transform.localScale = Vector3.zero;
                StartCoroutine(ScaleUp(obj.transform, targetScale));

                // HPとダストのステータス初期化
                if (hp != null)
                {
                   
                    hp.InitializeStatus(targetHealth, targetDastpar);
                }

                OrbitTarget orbit = obj.GetComponent<OrbitTarget>();
                if (orbit != null)
                {
                    orbit.player = player;
                }

                spawnedObjects.Add(obj);
                return;
            }
        }
    }

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
            maxRadius // 元のコードのまま（minRadiusのバグ防止用ならminRadiusに修正してください）
        );
    }
}