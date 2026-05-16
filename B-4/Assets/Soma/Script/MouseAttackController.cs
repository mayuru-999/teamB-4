using UnityEngine;
using UnityEngine.InputSystem;

public class MouseAttackController : MonoBehaviour
{
    public Player player;

    [Header("見た目")]
    public Transform attackRangeVisual;
    public Transform cooldownVisual;

    private float lastAttackTime = 0f;

    void Update()
    {
        if (player == null) return;
        if (Pointer.current == null || Camera.main == null) return;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(
            Pointer.current.position.ReadValue()
        );

        // 常にPlayerから取得
        float attackRadius = player.AttackRange;
        float attackInterval = player.AttackInterval;
        int attackPower = Mathf.RoundToInt(player.AttackPower);

        // 外側円（範囲）
        if (attackRangeVisual != null)
        {
            attackRangeVisual.position = mousePos;
            attackRangeVisual.localScale =
                new Vector3(attackRadius * 2f, attackRadius * 2f, 1f);
        }

        // クールタイム進行
        float progress = (Time.time - lastAttackTime) / attackInterval;
        progress = Mathf.Clamp01(progress);

        if (cooldownVisual != null)
        {
            cooldownVisual.position = mousePos;

            float currentRadius = attackRadius * progress;
            cooldownVisual.localScale =
                new Vector3(currentRadius * 2f, currentRadius * 2f, 1f);
        }

        // 攻撃
        if (progress >= 1f)
        {
            Attack(mousePos, attackRadius, attackPower);
            lastAttackTime = Time.time;

            if (cooldownVisual != null)
                cooldownVisual.localScale = Vector3.zero;
        }
    }

    void Attack(Vector2 center, float radius, int power)
    {
        Collider2D[] hits =
            Physics2D.OverlapCircleAll(center, radius);

        int count = 0;

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Target"))
            {
                TargetHP hp = hit.GetComponent<TargetHP>();
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