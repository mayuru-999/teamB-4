using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class SenceChang : MonoBehaviour
{
    public float changeTime = 30f;
    public TMP_Text timeText;
    public Image targetImage; // �F�ύX�p

    public Transform scaleImage;
    private Vector3 initialScale;

    // �ǉ��F���̉摜4��
    public Image[] borderImages;

    private float remainingTime;
    private bool isFinished = false;

    // �_�ŗp

    private float blinkInterval = 0.2f; // �_�ŊԊu
    private float blinkCounter = 0f;
    private bool isBlinkVisible = true;


    private float endDelay = 1f;     // 追加：遅延時間
    private float endTimer = 0f;
    private bool isEnding = false;


    void Start()
    {
        remainingTime = changeTime;
        initialScale = scaleImage.localScale;

        // �ŏ��͔�\��
        foreach (Image img in borderImages)
        {
            img.enabled = false;
        }

    }
    void Update()
    {
        if (isFinished) return;

        // 0秒後の待機処理
        if (isEnding)
        {
            endTimer += Time.deltaTime;

            if (endTimer >= endDelay)
            {
                isFinished = true;
                //SceneManager.LoadScene("SkillTree_debug");
                SceneManager.LoadScene("SkillTree");
            }

            return;
        }

        remainingTime -= Time.deltaTime;

        if (remainingTime <= 0)
        {
            remainingTime = 0;

            // すぐにシーン遷移しない
            isEnding = true;

            // ↓ここで攻撃を止める
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
            isBlinkVisible = !isBlinkVisible; // ON/OFF�؂�ւ�
        }

        foreach (Image img in borderImages)
        {
            img.enabled = isBlinkVisible;
        }

    }
}