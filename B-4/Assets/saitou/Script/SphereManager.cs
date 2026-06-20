using UnityEngine;

public class SphereManager : MonoBehaviour
{
    [System.Serializable]
    public class Ball
    {
        public RectTransform rect;

        // ▼ 入場アニメーション用の内部状態（インスペクタには出さない）
        [System.NonSerialized] public bool hasEntered;     // 入場が完了したか
        [System.NonSerialized] public float entryStartTime; // このボールの入場開始時刻
    }

    public RectTransform image;
    public Ball[] balls;
    public float radius = 250f; // 変更前250f
    public float verticalRatio = 0.5f; // 縦方向の圧縮率（1にすると真円に近づく）
    public float speed = 1f;
    public float minScale = 0.5f;
    public float maxScale = 1.5f;

    [Header("入場アニメーション設定")]
    public float entryInterval = 0.15f;  // ボールが入場を開始する間隔（インデックスごとのずれ）
    public float entryDuration = 1.0f;   // 1個あたりの入場にかかる時間（等速移動）
    public float entryOffscreenX = 900f; // 画面外左の開始X座標（絶対値。マイナス方向に配置される）

    private float startTime; // スクリプト開始時刻（入場タイミング計算の基準）

    void Start()
    {
        startTime = Time.time;
        for (int i = 0; i < balls.Length; i++)
        {
            balls[i].hasEntered = false;
            balls[i].entryStartTime = startTime + entryInterval * i;
        }
    }

    void Update()
    {
        for (int i = 0; i < balls.Length; i++)
        {
            var b = balls[i];

            // ■ 均等配置 + 自動回転（本来の角度はそのまま使う）
            float angle =
                (2f * Mathf.PI / balls.Length) * i
                + Time.time * speed;

            // ■ 円運動（楕円で立体感）― 本来の軌道上の位置（収束先）
            float targetX = Mathf.Cos(angle) * radius;
            float targetY = Mathf.Sin(angle) * radius * verticalRatio;

            float x, y = targetY; // Yは常に本来位置（高さは最初から合わせておく）

            if (!b.hasEntered)
            {
                if (Time.time < b.entryStartTime)
                {
                    // ■ まだ入場開始時刻に達していない
                    // → 画面外左で待機（高さだけ本来の軌道位置に揃えておく）
                    x = -entryOffscreenX;
                }
                else
                {
                    // ■ 入場中：画面外左 → 本来のX座標へ等速で近づける
                    float t = (Time.time - b.entryStartTime) / entryDuration;
                    if (t >= 1f)
                    {
                        t = 1f;
                        b.hasEntered = true; // 入場完了。以後は通常ロジックのみ通る
                    }
                    x = Mathf.Lerp(-entryOffscreenX, targetX, t);
                }
            }
            else
            {
                // ■ 入場完了後は通常の円運動
                x = targetX;
            }

            b.rect.anchoredPosition = new Vector2(x, y);

            // ■ 奥行き（修正版：上＝奥、下＝手前）
            // yの実際の範囲は ±radius*verticalRatio なので、それに合わせて正規化する
            float depth01 = 1f - ((targetY + radius * verticalRatio) / (radius * verticalRatio * 2f));

            // ■ スケール（奥小・手前大）
            float scale = Mathf.Lerp(minScale, maxScale, depth01);
            b.rect.localScale = Vector3.one * scale;

            // ■ 描画順（奥が上・手前が下）
            b.rect.SetSiblingIndex(Mathf.RoundToInt(depth01 * (balls.Length - 1)));

#if UNITY_EDITOR
            // エディタ上でのみ座標を確認したい場合はコメントを外す
            // Debug.Log($"{i}: {x}, {y}");
#endif
        }
    }
}