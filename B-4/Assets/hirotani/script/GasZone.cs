using UnityEngine;

public class GasZone : MonoBehaviour
{
    public float radius = 4f;
    public int damage = 5;
    public float interval = 1f;
    public float lifetime = 5f;

    private float timer;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            timer = 0f;
            ApplyDamage();
        }
    }

    void ApplyDamage()
    {
        Collider2D[] hits =
            Physics2D.OverlapCircleAll(transform.position, radius);

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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
