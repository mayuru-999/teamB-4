using UnityEngine;

// 指定されたタグを持つ全てのオブジェクトに対してダメージを与え、ビッグバン演出を管理するクラス
public class DeleteByTag : MonoBehaviour
{
    // 結果表示用パネル
    public GameObject resultPanel;

    private Renderer myRenderer;
    private Color originalColor;

    // 点滅に関する設定
    public float blinkSpeed = 5f;
    private bool isBlinking = false;

    // ビッグバンのパラメータ
    public string targetTag = "Target";
    public int damage = 10;
    public float attackInterval = 1f;
    private float timer = 0f;

    // 終了演出用
    public float endDelay = 1f;
    private bool isEnding = false;
    private float endTimer = 0f;

    // ビッグバン発動可能フラグ
    private bool canUseBigBang = false;

    void Start()
    {
        // 結果パネルを非表示で初期化
        if (resultPanel != null)
            resultPanel.SetActive(false);

        // マテリアルを複製して元の色を保持
        myRenderer = GetComponent<Renderer>();
        if (myRenderer != null)
        {
            myRenderer.material = new Material(myRenderer.material);
            originalColor = myRenderer.material.color;
        }

        // スキル管理クラスの初期化と訪問回数カウント
        if (SkillManage.Instance != null)
        {
            SkillManage.Instance.RefreshReferences();
            SkillManage.Instance.CheckAndIncrementVisitCount();
        }
    }

    void Update()
    {
        HandleBlink();

        // 終了処理中の場合はタイマーのみ更新して抜ける
        if (isEnding)
        {
            endTimer += Time.deltaTime;

            if (endTimer >= endDelay)
            {
                if (resultPanel != null)
                    resultPanel.SetActive(true);
            }

            return;
        }

        CheckBigBangReady();
        HandleInput();
    }

    // 点滅処理の更新
    void HandleBlink()
    {
        if (isBlinking && myRenderer != null)
        {
            float t = Mathf.Sin(Time.time * blinkSpeed) * 0.5f + 0.5f;
            Color blinkColor = Color.Lerp(originalColor, Color.red, t);
            myRenderer.material.color = blinkColor;
        }
    }

    // 点滅を停止して色を元に戻す
    void StopBlink()
    {
        isBlinking = false;

        if (myRenderer != null)
            myRenderer.material.color = originalColor;
    }

    // ビッグバン発動条件の判定
    void CheckBigBangReady()
    {
        if (SkillManage.Instance == null) return;

        if (SkillManage.Instance.MainVisitCount >= 1)
        {
            if (!canUseBigBang)
            {
                canUseBigBang = true;
                isBlinking = true;

                Debug.Log("ビッグバン準備OK");
            }
        }
    }

    // 入力処理（マウスホールド）
    void HandleInput()
    {
        if (Input.GetMouseButton(0))
        {
            if (canUseBigBang)
            {
                timer += Time.deltaTime;

                if (timer >= attackInterval)
                    TriggerBigBang();
            }
        }
        else
        {
            timer = 0f;
        }
    }

    // ビッグバン実行処理
    void TriggerBigBang()
    {
        AttackAll();
        timer = 0f;

        // スキル管理データの更新とリセット
        if (SkillManage.Instance != null)
        {
            SkillManage.Instance.LvUpdate();
            SkillManage.Instance.ResetVisitCount();
        }

        StopBlink();

        // 終了状態へ移行
        isEnding = true;
        canUseBigBang = false;
        MouseAttackController.canAttack = false;

        // 生成処理の停止
        if (Spawner.Instance != null)
        {
            Spawner.Instance.StopSpawning();
        }

        // シーン遷移管理への通知
        if (SenceChang.Instance != null)
        {
            SenceChang.Instance.OnBigBangTriggered();
        }

        Debug.Log("ビッグバン発動");
    }

    // タグに基づいた全体攻撃処理
    void AttackAll()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag(targetTag);

        foreach (GameObject obj in objs)
        {
            HPmanager hp = obj.GetComponent<HPmanager>();

            if (hp != null)
                hp.TakeDamage(damage, 2f);
        }

        // スキルデータのクリア
        if (SkillManage.Instance != null)
            SkillManage.Instance.ClearSkillData();

        Debug.Log("全体攻撃完了");
    }
}