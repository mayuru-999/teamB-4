using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Base Stats")]
    public float baseAttack = 1f;
    public float baseAttackInterval = 1.8f;
    public float baseAttackRange = 0.5f;

    [Header("Skill Reference")]
    [Tooltip("インスペクターから Critical Rate の .asset ファイルをドラッグ＆ドロップしてください")]
    [SerializeField] private SkillData criticalRateSkill;

    void Start()
    {
    }

    // 最終的なクリティカル率（％）を計算するプロパティ
    public float CriticalRate
    {
        get
        {
            if (SkillManage.Instance == null) return 0f;

            // スキルマネージャーからクリティカル率を取得
            float currentCritical = SkillManage.Instance.getEffect(SkillEffect.Type.ClitRate);

            // クリティカル率の最大値を 100% に制限
            return Mathf.Min(currentCritical, 100f);
        }
    }

    // 攻撃力
    public float AttackPower
    {
        get
        {
            if (SkillManage.Instance == null) return baseAttack;

            // 基本攻撃力 ＋ スキルによる加算値
            float currentAttack = baseAttack + SkillManage.Instance.getEffect(SkillEffect.Type.Attack);
            return currentAttack;
        }
    }

    // 攻撃インターバル
    public float AttackInterval
    {
        get
        {
            float interval = baseAttackInterval;

            if (SkillManage.Instance != null)
            {
                float percent = Mathf.Clamp(
                    SkillManage.Instance.getEffect(SkillEffect.Type.Speed) * 0.01f,
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

            float percent = SkillManage.Instance.getEffect(SkillEffect.Type.Range) * 0.01f;
            return baseAttackRange * (1 + percent);
        }
    }


    public int CalculateDamage(out bool isCritical)
    {
        // DeleteByTag から BigBang にクラス名を変更 
        if (BigBang.Instance != null && BigBang.Instance.IsWaitingForPanelClick)
        {
            isCritical = false;
            return 0; // 攻撃を発生させない、またはダメージを与えない
        }
        

        // 1. 最終的なダメージのベースを計算
        float finalDamage = AttackPower;
        isCritical = false;

        // 2. クリティカルの抽選 (0.0 から 100.0 のランダムな値を生成)
        float randomValue = Random.Range(0f, 100f);

        // 抽選された値が CriticalRate（％）以下ならクリティカル発動
        if (randomValue <= CriticalRate)
        {
            // クリティカル時はダメージ2倍
            finalDamage *= 2f;
            isCritical = true;

            // クリティカル発動時のデバッグログ
            Debug.Log($"<color=red>【Critical!】</color> クリティカルが発生しました！ (確率: {CriticalRate}%) ダメージ: {Mathf.RoundToInt(finalDamage)}");
        }
        else
        {
            // 通常ヒット時のデバッグログ
            Debug.Log($"通常ヒット: ダメージ {Mathf.RoundToInt(finalDamage)}");
        }

        return Mathf.RoundToInt(finalDamage);
    }


}