using UnityEngine;
using UnityEngine;

public class GasZone2D : MonoBehaviour
{
    public float radius = 4f;
    public int damage = 5;

    void Update()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D hit in hits)
        {
            if (!hit.CompareTag("Target")) continue;

            HPmanager hp = hit.GetComponent<HPmanager>();
            if (hp != null)
            {
                hp.TakeDamage(damage);
            }
        }
    }
}