using UnityEngine;

public class HPmanager : MonoBehaviour
{
    public int hp = 100;

    private int currentHP;

    [Header("ドロップアイテム")]
    public GameObject dropItemPrefab;

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
        if (dropItemPrefab != null)
        {
            Instantiate(
                dropItemPrefab,
                transform.position,
                Quaternion.identity
            );
        }

        Destroy(gameObject);
    }
}
