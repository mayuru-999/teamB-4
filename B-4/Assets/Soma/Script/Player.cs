using UnityEngine;

public class Player : MonoBehaviour
{

    [Header("Base Stats")]
    public float baseAttack = 1f;
    public float baseAttackInterval = 1.8f;
    public float baseAttackRange = 0.5f;

   
    void Start()
    {
    }

    // 攻撃力
    public float AttackPower
    {
        get
        {
            if (SkillManage.Instance == null) return baseAttack;

            return baseAttack
                + SkillManage.Instance.getEffect(SkillEffect.Type.Attack);
        }
    }
    //攻撃インターバル
    public float AttackInterval
    {
        get
        {
            float interval = baseAttackInterval;

            if (SkillManage.Instance != null)
            {
                float percent = Mathf.Clamp(
                    SkillManage.Instance.getEffect(SkillEffect.Type.Speed)*0.01f,
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
            if (SkillManage.Instance == null) return baseAttackRange;

            float percent =
                SkillManage.Instance.getEffect(SkillEffect.Type.Range) * 0.01f ;

            return baseAttackRange * (1 + percent);
        }
    }

}