using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class SenceChang : MonoBehaviour
{
    public float changeTime = 30f;
    public TMP_Text timeText;
    public Image targetImage;

    public Transform scaleImage;
    private Vector3 initialScale;

    public Image[] borderImages;

    public GameObject resultPanel; // ←追加

    private float remainingTime;
    private bool isFinished = false;

    private float blinkInterval = 0.2f;
    private float blinkCounter = 0f;
    private bool isBlinkVisible = true;

    private float endDelay = 1f;
    private float endTimer = 0f;
    private bool isEnding = false;

    void Start()
    {
        remainingTime = changeTime;
        initialScale = scaleImage.localScale;

        // 最初は非表示
        resultPanel.SetActive(false);

        foreach (Image img in borderImages)
        {
            img.enabled = false;
        }
    }

    void Update()
    {
        if (isFinished) return;

        if (isEnding)
        {
            endTimer += Time.deltaTime;

            if (endTimer >= endDelay)
            {
                isFinished = true;

                // シーン遷移の代わりにパネル表示
                resultPanel.SetActive(true);
            }
            return;
        }

        remainingTime -= Time.deltaTime;

        if (remainingTime <= 0)
        {
            remainingTime = 0;
            isEnding = true;

            // 攻撃停止
            MouseAttackController.canAttack = false;
        }

        timeText.text = remainingTime.ToString("F1") + "s";

        if (remainingTime <= 2f)
        {
            timeText.color = Color.red;
            targetImage.color = Color.red;
            BlinkBorders();
        }
        else
        {
            timeText.color = Color.white;
            targetImage.color = Color.white;

            foreach (Image img in borderImages)
            {
                img.enabled = false;
            }
        }

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
            isBlinkVisible = !isBlinkVisible;
        }

        foreach (Image img in borderImages)
        {
            img.enabled = isBlinkVisible;
        }
    }
}
