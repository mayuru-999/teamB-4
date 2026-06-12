using UnityEngine;
using TMPro;

public class SkillPointManager : MonoBehaviour
{
    public int skillPoint = 0;
    public TMP_Text scoreText;

    public static SkillPointManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


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

    public void UpdateUI()
    {
        if (scoreText == null) return;
        scoreText.text = "skillPoint: " + skillPoint;
    }
}
