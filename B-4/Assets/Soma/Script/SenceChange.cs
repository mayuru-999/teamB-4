using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SenceChang : MonoBehaviour
{
    public float changeTime = 30f; // 制限時間
    public TMP_Text timeText;

    private float remainingTime;
    private bool isFinished = false;

    void Start()
    {
        remainingTime = changeTime;
    }

    void Update()
    {
        if (isFinished) return;

        // 時間を減らす
        remainingTime -= Time.deltaTime;
        // ラスト5秒で色変更
        if (remainingTime <= 5f)
        {
            timeText.color = Color.red;
        }
        else
        {
            timeText.color = Color.white; // 通常色
        }
        if (remainingTime <= 0)
        {
            remainingTime = 0;
            isFinished = true;
            SceneManager.LoadScene("SkillTree");
        }

        // 表示更新
        timeText.text =  remainingTime.ToString("F1") + "s";
    }
}