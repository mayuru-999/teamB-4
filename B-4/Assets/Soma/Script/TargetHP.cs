using UnityEngine;

public class TargetHP : MonoBehaviour
{
    public int maxHP = 3;
    private int currentHP;

    public int skillPointValue = 1;

    public SkillPointManager skillPointManager;

    void Start()
    {
        currentHP = maxHP;

        if (skillPointManager == null)
        {
            skillPointManager = FindObjectOfType<SkillPointManager>();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
        {
            if (skillPointManager != null)
            {
                skillPointManager.AddScore(skillPointValue);
            }

            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}