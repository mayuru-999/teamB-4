using UnityEngine;

public class ChainDamage : MonoBehaviour
{
    [Header("自身のHP")]
    public int hp = 10;

    [Header("爆発設定")]
    public float radius = 5f;
    public int damage = 20;

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        /*
        if (sr != null)
        {
            sr.color = Color.yellow; // 特殊オブジェクトなので黄色
        }
        */
    }
        
    public void TakeDamage(int dmg)
    {
        hp -= dmg;

        if (hp <= 0)
        {
            Explode();
        }
    }

    public void Explode()
    {
        Collider2D[] hits =
            Physics2D.OverlapCircleAll(
                transform.position,
                radius
            );

        foreach (Collider2D hit in hits)
        {
            if (!hit.CompareTag("Target") || hit.gameObject == gameObject)
                continue;

            // ChainDamageが付いている敵にはダメージを与えない
            if (hit.GetComponent<ChainDamage>() != null)
                continue;

            HPmanager enemy = hit.GetComponent<HPmanager>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(
            transform.position,
            radius
        );
    }
}