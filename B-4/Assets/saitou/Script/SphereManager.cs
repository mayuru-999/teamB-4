using UnityEngine;

public class SphereManager : MonoBehaviour
{
    [System.Serializable]
    public class Ball
    {
        public RectTransform rect;

        [System.NonSerialized] public bool hasEntered;
        [System.NonSerialized] public float entryStartTime;
        [System.NonSerialized] public int enterOrder = -1;
        [System.NonSerialized] public bool entryStarted;
        [System.NonSerialized] public float entryAngleOffset;
    }

    public RectTransform image;
    public RectTransform ballsContainer;
    public Ball[] balls;
    public float radius = 250f;
    public float verticalRatio = 0.5f;
    public float speed = 1f;
    public float minScale = 0.5f;
    public float maxScale = 1.5f;

    [Header("入場アニメーション設定")]
    public float entryInterval = 0.02f;
    public float entryDuration = 1.0f;
    public float entryOffscreenX = 1400f;
    public float entrySpiralTurns = 0f;
    public float entryInitialDelay = 0f;

    private float startTime;
    private int nextEnterOrder;

    void Start()
    {
        startTime = Time.time;

        int n = balls.Length;
        int[] order = new int[n];
        float[] spotDistance = new float[n];

        float entryStartAngle = Mathf.PI;
        for (int i = 0; i < n; i++)
        {
            order[i] = i;
            float spotAngle = (2f * Mathf.PI / n) * i;
            float forward = (spotAngle - entryStartAngle) % (2f * Mathf.PI);
            if (forward < 0f) forward += 2f * Mathf.PI;
            spotDistance[i] = forward;
        }

        System.Array.Sort(order, (a, b) => spotDistance[b].CompareTo(spotDistance[a]));

        for (int rank = 0; rank < n; rank++)
        {
            int ballIndex = order[rank];
            balls[ballIndex].hasEntered = false;
            balls[ballIndex].enterOrder = -1;
            balls[ballIndex].entryStarted = false;
            balls[ballIndex].entryAngleOffset = 0f;
            balls[ballIndex].entryStartTime = startTime + entryInitialDelay + entryInterval * rank;
        }

        nextEnterOrder = 0;
    }

    void Update()
    {
        int n = balls.Length;
        float[] depths = new float[n];

        for (int i = 0; i < n; i++)
        {
            var b = balls[i];

            float targetAngle =
                (2f * Mathf.PI / n) * i
                + Time.time * speed;

            float targetX = Mathf.Cos(targetAngle) * radius;
            float targetY = Mathf.Sin(targetAngle) * radius * verticalRatio;

            float x, y;

            float startX = -entryOffscreenX;
            float startY = 0f;
            float startRadius = Mathf.Sqrt(
                startX * startX + (startY / Mathf.Max(verticalRatio, 0.0001f)) * (startY / Mathf.Max(verticalRatio, 0.0001f))
            );
            float startAngle = Mathf.Atan2(startY / Mathf.Max(verticalRatio, 0.0001f), startX);

            if (!b.hasEntered)
            {
                if (Time.time < b.entryStartTime)
                {
                    x = startX;
                    y = startY;
                }
                else
                {
                    if (!b.entryStarted)
                    {
                        // 入場開始時点の targetAngle から startAngle までの
                        // 反時計回りの総回転量を固定で記録する
                        float delta = targetAngle - startAngle;
                        delta = delta % (2f * Mathf.PI);
                        if (delta < 0f) delta += 2f * Mathf.PI;
                        float extraTurns = Mathf.Round(entrySpiralTurns);
                        b.entryAngleOffset = delta + extraTurns * 2f * Mathf.PI;
                        b.entryStarted = true;
                    }

                    float t = (Time.time - b.entryStartTime) / entryDuration;
                    if (t >= 1f)
                    {
                        t = 1f;
                        b.hasEntered = true;
                        b.enterOrder = nextEnterOrder;
                        nextEnterOrder++;
                    }

                    // EaseOutQuad：終点に近づくほど減速する
                    float easedT = 1f - (1f - t) * (1f - t);

                    float currentRadius = Mathf.Lerp(startRadius, radius, easedT);

                    // 終点は常に最新の targetAngle に追従。
                    // 回転量は入場開始時に固定した entryAngleOffset を使う。
                    float currentAngle = targetAngle - b.entryAngleOffset * (1f - easedT);

                    x = Mathf.Cos(currentAngle) * currentRadius;
                    y = Mathf.Sin(currentAngle) * currentRadius * verticalRatio;
                }
            }
            else
            {
                x = targetX;
                y = targetY;
            }

            b.rect.anchoredPosition = new Vector2(x, y);

            float depth01 = 1f - ((y + radius * verticalRatio) / (radius * verticalRatio * 2f));
            depth01 = Mathf.Clamp01(depth01);
            depths[i] = depth01;

            float scale = Mathf.Lerp(minScale, maxScale, depth01);
            b.rect.localScale = Vector3.one * scale;
        }

        // ■ 2パス目：重なり順（SiblingIndex）の決定
        int[] sortOrder = new int[n];
        for (int i = 0; i < n; i++) sortOrder[i] = i;

        System.Array.Sort(sortOrder, (a, b) =>
        {
            return depths[a].CompareTo(depths[b]);
        });

        for (int rank = 0; rank < n; rank++)
        {
            balls[sortOrder[rank]].rect.SetSiblingIndex(rank);
        }
    }
}