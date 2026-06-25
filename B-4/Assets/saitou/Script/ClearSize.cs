using UnityEngine;

public class ImageSizeAnimator2 : MonoBehaviour
{
    [Header("対象")]
    public RectTransform target;

    [Header("サイズ設定")]
    public float minScale = 1.0f;
    public float maxScale = 1.5f;
    public float duration = 2.0f;

    [Header("動作モード")]
    public bool pingPong = false;

    [Header("待機時間")]
    public float delay = 1.0f;

    private float timer = 0f;
    private Vector2 originalSize;

    void Start()
    {
        if (target == null)
            target = GetComponent<RectTransform>();

        originalSize = target.sizeDelta;

        // 最初から大きい状態にしておく
        target.sizeDelta = originalSize * maxScale;
    }

    void Update()
    {
        timer += Time.deltaTime;

        // 待機時間中は大きいまま維持（何もしない）
        if (timer < delay) return;

        float animTimer = timer - delay;

        float t;
        if (pingPong)
        {
            t = Mathf.PingPong(animTimer / duration, 1f);
        }
        else
        {
            t = Mathf.Clamp01(animTimer / duration);
        }

        float smoothT = Mathf.SmoothStep(0f, 1f, t);
        float scale = Mathf.Lerp(maxScale, minScale, smoothT);

        target.sizeDelta = originalSize * scale;
    }
}