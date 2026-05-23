using UnityEngine;

public class HPmanager : MonoBehaviour
{
    public int hp = 100;
    private int currentHP;

    [Header("ドロップアイテム")]
    public DropItemData[] dropItems;

    void Start()
    {
        currentHP = hp;
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