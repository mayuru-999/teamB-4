using UnityEngine;

public class TargetHP : MonoBehaviour
{
    public int maxHP = 3;
    private int currentHP;

    public int skillPointValue = 1;      // 倒したときのスコア
    public ScoreManager scoreManager; // Inspectorでセット

    void Start()
    {
        currentHP = maxHP;

        if (scoreManager == null)
        {
            scoreManager = FindObjectOfType<ScoreManager>();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
        {
            scoreManager.AddScore(skillPointValue); // ←ここでスコア加算
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}