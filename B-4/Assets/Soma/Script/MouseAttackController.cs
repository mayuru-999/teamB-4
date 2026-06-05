using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MouseAttackController : MonoBehaviour
{
    public Player player;

    [Header("見た目 (UI Image)")]
    public Image attackRangeVisual;
    public Image cooldownVisual;

    [Header("サイズ個別の調整")]
    [Tooltip("攻撃範囲（外枠）の見た目の大きさ倍率")]
    public float rangeScaleMultiplier = 0.9f; 
    
    [Tooltip("クールタイム（内側ゲージ）の見た目の大きさ倍率。小さくしたい場合は数値を下げます")]
    public float cooldownScaleMultiplier = 1.0f; // ★最初から少し小さめ（0.8倍）に設定

    private float lastAttackTime = 0f;

    void Update()
    {
        if (player == null) return;
        if (Pointer.current == null || Camera.main == null) return;

        Vector2 screenPos = Pointer.current.position.ReadValue();
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

        float attackRadius = player.AttackRange;
        float attackInterval = player.AttackInterval;
        int attackPower = Mathf.RoundToInt(player.AttackPower);

        // --- 1. 外側円（範囲ガイド）の処理 ---
        if (attackRangeVisual != null)
        {
            attackRangeVisual.transform.position = worldPos;
            
            // 攻撃範囲用のサイズ計算（rangeScaleMultiplier を掛ける）
            float rangeVisualSize = (attackRadius * 2f) * rangeScaleMultiplier;
            attackRangeVisual.rectTransform.localScale = 
                new Vector3(rangeVisualSize, rangeVisualSize, 1f);
        }

        // --- 2. クールタイム進行度（0f ～ 1f）の計算 ---
        float progress = (Time.time - lastAttackTime) / attackInterval;
        progress = Mathf.Clamp01(progress);

        // --- 3. クールタイムゲージ（Filled）の処理 ---
        if (cooldownVisual != null)
        {
            cooldownVisual.transform.position = worldPos;
            
            // ★クールタイム専用のサイズ計算（cooldownScaleMultiplier を掛ける）
            float cooldownVisualSize = (attackRadius * 2f) * cooldownScaleMultiplier;
            cooldownVisual.rectTransform.localScale = 
                new Vector3(cooldownVisualSize, cooldownVisualSize, 1f);

            cooldownVisual.fillAmount = progress;
        }

        // --- 4. 攻撃の実行 ---
        if (progress >= 1f)
        {
            // ※実際の攻撃判定は、倍率をかける前の本来の半径（attackRadius）で行われます
            Attack(worldPos, attackRadius, attackPower);
            lastAttackTime = Time.time;

            if (cooldownVisual != null)
            {
                cooldownVisual.fillAmount = 0f;
            }
        }
    }

    void Attack(Vector2 center, float radius, int power)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(center, radius);
        int count = 0;

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Target"))
            {
                HPmanager hp = hit.GetComponent<HPmanager>();

                if (hp != null)
                {
                    hp.TakeDamage(power);
                    count++;
                }
            }
        }

        Debug.Log(count + " 個にダメージ");
    }

    void OnDrawGizmos()
    {
        if (Pointer.current == null || Camera.main == null || player == null)
            return;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(
            Pointer.current.position.ReadValue()
        );

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(mousePos, player.AttackRange);
    }
}