using System.Collections;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    // 揺らす対象のカメラ（未設定の場合はメインカメラを自動取得）
    [SerializeField] private Transform cameraTransform;

    // 揺れの初期位置を保存する変数
    private Vector3 originalPosition;

    void Awake()
    {
        if (cameraTransform == null)
        {
            if (Camera.main != null)
                cameraTransform = Camera.main.transform;
            else
                cameraTransform = transform;
        }
    }

    void Start()
    {
        // シーン開始時に速攻で揺らしたい場合はここで呼び出す
        TriggerShake(0.5f, 0.2f);
    }

    /// <summary>
    /// 外部のスクリプトからこのメソッドを呼ぶことで、いつでも揺らせます
    /// </summary>
    /// <param name="duration">揺れる時間（秒）</param>
    /// <param name="magnitude">揺れの強さ</param>
    public void TriggerShake(float duration, float magnitude)
    {
        // すでに揺れている可能性を考慮し、一度コルーチンを止めてから再スタート
        StopAllCoroutines();
        StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        // 揺れ始める前のローカル位置を記憶
        originalPosition = cameraTransform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            // ランダムな方向にずらす位置を計算
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            // カメラのローカル位置をずらす（Z軸はそのまま保持）
            cameraTransform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

            elapsed += Time.deltaTime;

            // 1フレーム待機
            yield return null;
        }

        // 揺れが終わったら、完全に元の位置に戻す
        cameraTransform.localPosition = originalPosition;
    }
}