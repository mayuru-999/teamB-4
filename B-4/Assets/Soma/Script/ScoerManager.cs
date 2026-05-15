using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public int skillPoint = 0;
    public TMP_Text scoreText;

    void Start()
    {
        if (scoreText == null)
        {
            scoreText = FindObjectOfType<TMP_Text>();
        }

        UpdateUI();
    }

    public void AddScore(int value)
    {
        skillPoint += value;
        UpdateUI();
    }

    void UpdateUI()
    {
        scoreText.text = "skillPoint: " + skillPoint;
    }
}
