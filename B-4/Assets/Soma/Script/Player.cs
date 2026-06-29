using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Base Stats")]
    public float baseAttack = 1f;
    public float baseAttackInterval = 1.8f;
    public float baseAttackRange = 0.5f;

    [Header("Skill Reference")]
    // ★ ここに、画像の「Clitical Rate (.asset)」ファイルをインスペクターからドラッグ＆ドロップしてください
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

            // 1. スキルマネージャーが自動で集計したクリティカル率を取得 (画像の設定通りなら解放時に10が入る)
            float currentCritical = SkillManage.Instance.getEffect(SkillEffect.Type.ClitRate);

            // 2. もし「画像の特定のスキル」が解放されている場合、さらに個別に10%付与したい場合の処理
            // (※もしgetEffect側で既に10%乗っていて、二重に足したくない場合はこのif文は不要です)
            /*
            if (criticalRateSkill != null && SkillManage.Instance.isUnlocked(criticalRateSkill))
            {
                // 個別にさらに加算したい場合はここに書く
                // currentCritical += 10f; 
            }
            */

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

            float percent =
                SkillManage.Instance.getEffect(SkillEffect.Type.Range) * 0.01f;

            return baseAttackRange * (1 + percent);
        }
    }
}