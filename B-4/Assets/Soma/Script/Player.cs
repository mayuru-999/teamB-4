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
    //攻撃インターバル
    public float AttackInterval
    {
        get
        {
            float interval = baseAttackInterval;

            if (skillManage != null)
            {
                float percent = Mathf.Clamp(
                    skillManage.getEffect(SkillEffect.Type.Speed),
                    0f,
                    0.95f
                );

                interval *= (1 - percent);
            }

            return Mathf.Max(interval, 0.05f);
        }
    }



    // 攻撃範囲
    public float AttackRange
    {
        get
        {
            if (skillManage == null) return baseAttackRange;

            float percent =
                skillManage.getEffect(SkillEffect.Type.Range);

            return baseAttackRange * (1 + percent);
        }
    }

}