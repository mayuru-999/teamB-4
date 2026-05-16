using UnityEngine;

public class Player : MonoBehaviour
{
    public SkillManage skillManage;

    [Header("Base Stats")]
    public float baseAttack = 1f;
    public float baseAttackInterval = 1.8f;
    public float baseAttackRange = 0.5f;

   
    void Start()
    {
        skillManage = FindObjectOfType<SkillManage>();
    }

    // 攻撃力
    public float AttackPower
    {
        get
        {
            if (skillManage == null) return baseAttack;

            return baseAttack
                + skillManage.getEffect(SkillEffect.Type.Attack);
        }
    }

    public float AttackInterval
    {
        get
        {
            float interval = baseAttackInterval;

            if (skillManage != null)
            {
                interval -= skillManage.getEffect(SkillEffect.Type.Speed);
            }

            // 最低値制限（バグ防止）
            return Mathf.Max(interval, 0.05f);
        }
    }


    // 攻撃範囲
    public float AttackRange
    {
        get
        {
            if (skillManage == null) return baseAttackRange;

            return baseAttackRange
                + skillManage.getEffect(SkillEffect.Type.Range);
        }
    }
}