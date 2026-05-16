using UnityEngine;

public class TergetHP2 : MonoBehaviour
{
    [Header("HP")]
    public int maxHP = 3;

    private int currentHP;

    [Header("ドロップアイテム")]
    public GameObject dropItemPrefab;

    void Start()
    {
        currentHP = maxHP;
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
        // アイテムドロップ
        if (dropItemPrefab != null)
        {
            Instantiate(
                dropItemPrefab,
                transform.position,
                Quaternion.identity
            );
        }

        // 自分を削除
        Destroy(gameObject);
    }
}
