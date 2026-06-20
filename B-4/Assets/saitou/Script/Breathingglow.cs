using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI Image を「呼吸するように」明滅させるスクリプト。
/// Bloom が効くカメラ（Screen Space - Camera など）配下の Image にアタッチして使用します。
/// Image の Color を HDR（1.0以上）にしてサイン波で明るさを揺らすことで、
/// Global Volume の Bloom がそれに反応して自然に光って見えます。
/// </summary>
[RequireComponent(typeof(Image))]
public class BreathingGlow : MonoBehaviour
{
    [Header("点滅させたいベースカラー（RGB成分の比率）")]
    [Tooltip("光の色味。明るさは下のIntensityで別途制御するので、ここではRGBの比率だけ意味を持ちます。")]
    [SerializeField] private Color baseColor = Color.white;

    [Header("明るさの範囲（HDR強度）")]
    [Tooltip("最も暗い時の明るさ倍率。1.0以下だとBloomがかからない場合があります。")]
    [SerializeField] private float minIntensity = 1.0f;

    [Tooltip("最も明るい時の明るさ倍率。BloomのThresholdより大きい値にする必要があります。")]
    [SerializeField] private float maxIntensity = 3.0f;

    [Header("呼吸のスピード")]
    [Tooltip("1往復にかかる時間（秒）。大きいほどゆっくり呼吸します。")]
    [SerializeField] private float cycleDuration = 2.5f;

    [Header("アルファ値（透明度）")]
    [Tooltip("Imageの透明度。基本的に1（不透明）でOKです。")]
    [SerializeField] private float alpha = 1.0f;

    private Image targetImage;
    private float timer;

    private void Awake()
    {
        targetImage = GetComponent<Image>();
    }

    private void Update()
    {
        // 0→1→0 と滑らかに往復するサイン波（呼吸のような変化）
        timer += Time.deltaTime;
        float t = (Mathf.Sin((timer / cycleDuration) * Mathf.PI * 2f) + 1f) * 0.5f;

        float intensity = Mathf.Lerp(minIntensity, maxIntensity, t);

        Color glowColor = baseColor * intensity;
        glowColor.a = alpha;

        targetImage.color = glowColor;
    }
}