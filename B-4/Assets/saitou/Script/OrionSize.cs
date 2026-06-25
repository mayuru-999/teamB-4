using UnityEngine;

public class ImageSizeAnimator : MonoBehaviour
{
    [Header("対象")]
    public RectTransform target; // 拡大縮小させたいImageのRectTransform

    [Header("サイズ設定")]
    public float minScale = 1.0f;   // 最小スケール
    public float maxScale = 1.5f;   // 最大スケール
    public float duration = 2.0f;   // 最小→最大までにかかる時間（秒）

    [Header("動作モード")]
    public bool pingPong = false;    // true: 大きくなったら小さくなる(繰り返し) / false: 一回だけ

    private float timer = 0f;

    void Start()
    {
        if (target == null)
            target = GetComponent<RectTransform>();

    }

    void Update()
    {
        timer += Time.deltaTime;

        float t;
        if (pingPong)
        {
            // 0→1→0→1...を繰り返す
            t = Mathf.PingPong(timer / duration, 1f);
        }
        else
        {
            // 0→1で止まる
            t = Mathf.Clamp01(timer / duration);
        }

        // なめらかにするためSmoothStepを使用（お好みでLerpでも可）
        float smoothT = Mathf.SmoothStep(0f, 1f, t);
        float scale = Mathf.Lerp(maxScale, minScale, smoothT);

        target.localScale = new Vector3(scale, scale, 1f);
    }
}