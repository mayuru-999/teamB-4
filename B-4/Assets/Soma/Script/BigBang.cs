using UnityEngine;

// 指定されたタグを持つ全てのオブジェクトに対してダメージを与え、ビッグバン演出を管理するクラス
public class BigBang : MonoBehaviour
{
    // 外部から簡単にアクセスできるようにインスタンスを保持（シングルトン化）
    public static BigBang Instance { get; private set; }

    // 結果表示用パネル
    public GameObject resultPanel;

    [SerializeField] private GameObject reticleCanvasObject;
    // 初めて条件をクリアしたときに表示する説明用パネル
    [Header("チュートリアル設定")]
    public GameObject firstClearPanel;
    public bool IsWaitingForPanelClick { get; private set; } = false; // パネルのクリック待ち状態フラグ

    [Header("ビッグバン表示設定")]
    //  ビッグバン条件がtrueの時に表示したい画像（GameObject）
    public GameObject bigBangImage;

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
    [System.NonSerialized] public bool canUseBigBang = false;

    void Awake()
    {
        // インスタンスの登録
        if (Instance == null) { Instance = this; }
    }

    void Start()
    {
        // 結果パネルを非表示で初期化
        if (resultPanel != null)
            resultPanel.SetActive(false);

        // 初期化時に初めて用パネルを非表示にしておく
        if (firstClearPanel != null)
            firstClearPanel.SetActive(false);

        // 初期化時にビッグバン画像を非表示にしておく
        if (bigBangImage != null)
            bigBangImage.SetActive(false);

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

        // パネル表示中のクリック待ち処理
        if (IsWaitingForPanelClick)
        {
            //// 左クリックが押されたらパネルを閉じて再開
            //if (Input.GetMouseButtonDown(0))
            //{
            //    if (firstClearPanel != null)
            //        firstClearPanel.SetActive(false);

            //    // ゲームを再開する（タイマーを動かし、スポナーを再稼働する）
            //    if (SenceChang.Instance != null)
            //        SenceChang.Instance.SetTimerPause(false);

            //    if (Spawner.Instance != null)
            //        Spawner.Instance.ResumeSpawning();

            //    IsWaitingForPanelClick = false;

            //    // チュートリアルを閉じたら、本来出すべきだったビッグバン画像を表示する
            //    if (bigBangImage != null && canUseBigBang)
            //        bigBangImage.SetActive(true);

            //    if (reticleCanvasObject != null)
            //        reticleCanvasObject.SetActive(true);

            //}
            return; // パネルが表示されている間は、これ以降のビッグバン長押し入力を受け付けない
        }

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

    // ビッグバン発動条件の判定
    void CheckBigBangReady()
    {
        if (SkillManage.Instance == null) return;

        // 条件が満たされたとき
        if (SkillManage.Instance.MainVisitCount >= 1)
        {
            if (!canUseBigBang)
            {
                canUseBigBang = true;
                isBlinking = true;

                Debug.Log("ビッグバン準備OK");

                // 本当に「最初の1回目」の時だけパネルを出して全てを止める
                if (!SkillManage.Instance.HasTriggeredFirstStop)
                {
                    //// パネルを表示
                    //if (firstClearPanel != null)
                    //    firstClearPanel.SetActive(true);

                    //// スポナーを止める
                    //if (Spawner.Instance != null)
                    //    Spawner.Instance.StopSpawning();

                    //// 画面上のタイマー（時間）もストップさせる 
                    //if (SenceChang.Instance != null)
                    //    SenceChang.Instance.SetTimerPause(true);

                    //// クリック待ち状態フラグを立てる
                    //IsWaitingForPanelClick = true;

                    //if (reticleCanvasObject != null)
                    //    reticleCanvasObject.SetActive(false);

                    //SkillManage.Instance.HasTriggeredFirstStop = true;
                    //Debug.Log("【演出】初回限定のパネル表示・スポナー＆タイマー停止を実行しました。");
                }
                else
                {

                    if (bigBangImage != null)
                        bigBangImage.SetActive(true);

                    if (reticleCanvasObject != null)
                    {
                        reticleCanvasObject.SetActive(false);

                    }
                }
            }
        }
    }

    void StopBlink()
    {
        isBlinking = false;

        if (myRenderer != null)
            myRenderer.material.color = originalColor;
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

        // ビッグバンが発動したので画像を非表示にする
        if (bigBangImage != null)
            bigBangImage.SetActive(false);

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

        if (reticleCanvasObject != null)
            reticleCanvasObject.SetActive(true);

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

    //dekura_tuika
    public void theWorld(bool value)
    {
        IsWaitingForPanelClick = value;

        if (value)
        {
            // スポナーを止める
            if (Spawner.Instance != null)
                Spawner.Instance.StopSpawning();

            // 画面上のタイマー（時間）もストップさせる 
            if (SenceChang.Instance != null)
                SenceChang.Instance.SetTimerPause(true);

            Debug.Log("【演出】初回限定のパネル表示・スポナー＆タイマー停止を実行しました。");
        }
        else
        {
            if (SenceChang.Instance != null)
                SenceChang.Instance.SetTimerPause(false);

            if (Spawner.Instance != null)
                Spawner.Instance.ResumeSpawning();

            if (bigBangImage != null && canUseBigBang)
                bigBangImage.SetActive(true);

            Debug.Log("【演出】チュートリアルパネルが閉じられたため、ゲームを再開します。");
        }
    }
}