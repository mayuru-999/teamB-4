using UnityEngine;

public class TargettHP : MonoBehaviour
{
    public int maxHP = 3;

    private int currentHP;

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
            Destroy(gameObject);
        }
    }
}
