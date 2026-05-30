using UnityEngine;

public class HPmanager : MonoBehaviour
{
    public int hp = 100;
    private int currentHP;

    [Header("ドロップアイテム")]
    public DropItemData[] dropItems;
    [Header("特殊ドロップ")]
    public GameObject chainDamagePrefab;

    [Range(0f, 1f)]
    public float chainDropRate = 0.1f;
    public bool isSpecial = false;

    void Start()
    {
        currentHP = hp;

        if (isSpecial)
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();

            if (sr != null)
            {
                sr.color = Color.yellow;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        Debug.Log(gameObject.name + " HP : " + currentHP);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isSpecial)
        {
            Collider2D[] hits =
                Physics2D.OverlapCircleAll(
                    transform.position,
                    5f
                );

            foreach (Collider2D hit in hits)
            {
                if (hit.CompareTag("Target"))
                {
                    HPmanager hp =
                        hit.GetComponent<HPmanager>();

                    if (hp != null &&
                         hp != this &&
                          !hp.isSpecial)
                    {
                        hp.TakeDamage(20);
                    }
                }
            }
        }

        foreach (var item in dropItems)
        {
            if (item != null && item.prefab != null)
            {
                for (int i = 0; i < item.count; i++)
                {
                    Instantiate(
                        item.prefab,
                        transform.position,
                        Quaternion.identity
                    );
                }
            }
        }

        Destroy(gameObject);
    }
}