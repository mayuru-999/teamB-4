using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class SenceChang : MonoBehaviour
{
    public float changeTime = 30f;
    public TMP_Text timeText;
    public Image targetImage; // 色変更用

    public Transform scaleImage;
    private Vector3 initialScale;

    // 追加：縁の画像4つ
    public Image[] borderImages;

    private float remainingTime;
    private bool isFinished = false;

    // 点滅用

    private float blinkInterval = 0.2f; // 点滅間隔
    private float blinkCounter = 0f;
    private bool isBlinkVisible = true;

 

    void Start()
    {
        remainingTime = changeTime;
        initialScale = scaleImage.localScale;

        // 最初は非表示
        foreach (Image img in borderImages)
        {
            img.enabled = false;
        }

    }

    void Update()
    {
        if (isFinished) return;

        remainingTime -= Time.deltaTime;

        if (remainingTime <= 0)
        {
            remainingTime = 0;
            isFinished = true;
            SceneManager.LoadScene("SkillTree");
        }

        // 表示
        timeText.text = remainingTime.ToString("F1") + "s";

        // 残り2秒で赤
        if (remainingTime <= 2f)
        {
            timeText.color = Color.red;
            targetImage.color = Color.red;

            // 点滅処理
            BlinkBorders();
        }
        else
        {
            timeText.color = Color.white;
            targetImage.color = Color.white;

            // 常に非表示にする
            foreach (Image img in borderImages)
            {
                img.enabled = false;
            }
        }

        // スケール縮小
        float rate = remainingTime / changeTime;
        Vector3 newScale = initialScale;
        newScale.x = initialScale.x * rate;
        scaleImage.localScale = newScale;
    }


    void BlinkBorders()
    {
        blinkCounter += Time.deltaTime;

        if (blinkCounter >= blinkInterval)
        {
            blinkCounter = 0f;
            isBlinkVisible = !isBlinkVisible; // ON/OFF切り替え
        }

        foreach (Image img in borderImages)
        {
            img.enabled = isBlinkVisible;
        }

    }
}