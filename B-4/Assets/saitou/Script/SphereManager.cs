using UnityEngine;

public class SphereManager : MonoBehaviour
{
    [System.Serializable]
    public class Ball
    {
        public RectTransform rect;
    }

    public Ball[] balls;

    public float radius = 250f;
    public float speed = 1f;

    public float minScale = 0.5f;
    public float maxScale = 1.5f;

    void Update()
    {
        for (int i = 0; i < balls.Length; i++)
        {
            var b = balls[i];

            // ■ 均等配置 + 自動回転
            float angle =
                (2f * Mathf.PI / balls.Length) * i
                + Time.time * speed;

            // ■ 円運動（楕円で立体感）
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius * 0.5f;

            b.rect.anchoredPosition = new Vector2(x, y);

            // ■ 奥行き（修正版：上＝奥、下＝手前）
            float depth01 = 1f - ((y + radius) / (2f * radius));

            // ■ スケール（奥小・手前大）
            float scale = Mathf.Lerp(minScale, maxScale, depth01);
            b.rect.localScale = Vector3.one * scale;

            // ■ 描画順（奥が上・手前が下）
            b.rect.SetSiblingIndex(Mathf.RoundToInt(depth01 * 1000));
        }
    }
}