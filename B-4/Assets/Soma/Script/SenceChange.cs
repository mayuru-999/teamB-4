using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class SenceChang : MonoBehaviour
{
    // 外部から簡単にアクセスできるようにインスタンスを保持（シングルトン化）
    public static SenceChang Instance { get; private set; }

    public float changeTime = 30f;
    public TMP_Text timeText;
    public Image targetImage;

    public Transform scaleImage;
    private Vector3 initialScale;

    public Image[] borderImages;

    public GameObject resultPanel;

    private float remainingTime;
    private bool isFinished = false;

    private float blinkInterval = 0.2f;
    private float blinkCounter = 0f;
    private bool isBlinkVisible = true;

    public float endDelay = 1f; // 外から調整できるように public に変更
    private float endTimer = 0f;
    private bool isEnding = false;

    void Awake()
    {
        // インスタンスの登録
        if (Instance == null) { Instance = this; }
    }

    void Start()
    {
        remainingTime = changeTime;
        initialScale = scaleImage.localScale;

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
                resultPanel.SetActive(true);
            }
            return;
        }

        remainingTime -= Time.deltaTime;

        if (remainingTime <= 0)
        {
            TriggerEndSequence(); // 終了処理を関数に共通化
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

    // タイムアップ時とビッグバン時で共通する終了処理
    private void TriggerEndSequence()
    {
        remainingTime = 0;
        isEnding = true;
        MouseAttackController.canAttack = false;
    }

    // ★追加: ビッグバンが起きた時に外部から呼び出す関数
    public void OnBigBangTriggered()
    {
        if (isFinished || isEnding) return;

        // タイマーのテキストを「0.0s」や「CLEAR!」などに変更したい場合はここ
        // timeText.text = "0.0s"; 

        // 強制的に終了シーエンス（ディレイカウント）へ移行させる
        TriggerEndSequence();
    }
}