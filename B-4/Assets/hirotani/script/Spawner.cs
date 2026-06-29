using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    // --- 追加: 生成をコントロールするシングルトン or フラグ ---
    public static Spawner Instance { get; private set; }
    private bool isSpawningActive = true;
    // ----------------------------------------------------

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

    void Awake()
    {
        // 外部からアクセスしやすいようにシングルトン化
        if (Instance == null) { Instance = this; }
    }

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

    // --- 追加: 外部から生成を止めるためのメソッド ---
    public void StopSpawning()
    {
        isSpawningActive = false;
        Debug.Log("Spawner: 新しい惑星の生成を停止しました。");
    }
    // ------------------------------------------------

    System.Collections.IEnumerator ScaleUp(Transform t, Vector3 target)
    {
        float time = 0f;
        float duration = 0.4f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float ratio = time / duration;

            float scale = Mathf.Sin(ratio * Mathf.PI * 0.5f)
                        + Mathf.Sin(ratio * Mathf.PI) * 0.15f;

            t.localScale = target * scale;

            yield return null;
        }

        t.localScale = target;
    }


    void SpawnUntilKeepCount()
    {
        // --- 追加: 生成停止フラグが立っていたら処理を行わない ---
        if (!isSpawningActive) return;
        // ----------------------------------------------------

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
       
        if (!isSpawningActive) return;
     

        if (player == null || targetPrefabs.Length == 0) return;

        // 最大試行回数（無限ループ防止）
        for (int attempt = 0; attempt < 20; attempt++)
        {
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            float randomDistance = Random.Range(minRadius, maxRadius);

            Vector3 spawnPosition = new Vector3(
                Mathf.Cos(angle) * randomDistance,
                Mathf.Sin(angle) * randomDistance,
                0
            ) + player.position;

            // 重なりチェック
            Collider2D hit = Physics2D.OverlapCircle(spawnPosition, checkRadius);

            if (hit == null)
            {

                Vector3 spawnRates = Vector3.right; // デフォルト (1, 0, 0)
                if (SkillManage.Instance != null)
                {
                    spawnRates = SkillManage.Instance.getPlaneSizeLv(); // x=小の確率, y=中の確率, z=大の確率
                    Debug.Log("Lv" + SkillManage.Instance.getPlaneSizeLv());
                }

                int targetIndex = 0; // デフォルトは 0 (サイズ1・小)
                float roll = Random.value; // 0.0 〜 1.0 のランダム値
                Debug.Log("roll" + roll);
                Debug.Log(spawnRates.x + " " + spawnRates.y + " " + spawnRates.z);
                if (roll < spawnRates.x)
                {
                    Debug.Log(0);
                    targetIndex = 0; // サイズ1（小）
                }
                else if (roll < spawnRates.x + spawnRates.y)
                {
                    Debug.Log(1);
                    targetIndex = 1; // サイズ2（中）
                }
                else
                {
                    Debug.Log(2);
                    targetIndex = 2; // サイズ3（大）
                }

                // 配列の要素数を超えないように安全ガード（念のため）
                if (targetIndex >= targetPrefabs.Length)
                {
                    targetIndex = targetPrefabs.Length - 1;
                }

                GameObject prefabToSpawn;

                // 10%で特殊敵
                if (specialTargetPrefabs.Length > 0 && Random.value < specialSpawnRate)
                {
                    // 特殊敵の配列からも、同じサイズ（インデックス）のものを引っ張る
                    int specialIndex = Mathf.Min(targetIndex, specialTargetPrefabs.Length - 1);
                    prefabToSpawn = specialTargetPrefabs[specialIndex];
                }
                else
                {
                    prefabToSpawn = targetPrefabs[targetIndex];
                }

                GameObject obj = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

                // 各種ステータスの初期化用変数
                Vector3 targetScale = Vector3.one * 0.4f;
                Vector3 targetHealth = Vector3.one * 3f;
                Vector3 targetDastpar = Vector3.one * 5f;

                HPmanager hp = obj.GetComponent<HPmanager>();

                if (SkillManage.Instance != null && hp != null)
                {
                    // レベルに応じたステータスを取得
                    var (currentHealth, currentCrystal, currentDust) = SkillManage.Instance.LvtoPlaneData();
                    targetHealth = currentHealth;
                    targetDastpar = currentDust;

                    if (targetIndex == 0)
                    {
                        hp.sizeType = 1;
                        targetScale = Vector3.one * 0.4f; // サイズ1の見た目の大きさ
                    }
                    else if (targetIndex == 1)
                    {
                        hp.sizeType = 2;
                        targetScale = Vector3.one * 0.6f; // サイズ2の見た目の大きさ
                    }
                    else
                    {
                        hp.sizeType = 3;
                        targetScale = Vector3.one * 0.8f; // サイズ3の見た目の大きさ
                    }
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
            minRadius // minRadiusのバグ表示を修正
        );
    }
}