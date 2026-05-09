using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TargetClearer : MonoBehaviour
{
    public Transform attackVisual;
    public int attackPower = 1;
    public float explosionRadius = 1.0f;
    public float attackInterval = 1.8f;

    private float lastAttackTime = 0f;

    void Update()
    {
        if (Pointer.current == null || Camera.main == null) return;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Pointer.current.position.ReadValue());

        // 位置追従
        if (attackVisual != null)
        {
            attackVisual.position = mousePos;
        }

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(mousePos, explosionRadius);

        HashSet<GameObject> currentTargets = new HashSet<GameObject>();

        foreach (Collider2D hit in hitColliders)
        {
            if (hit.CompareTag("Target"))
            {
                currentTargets.Add(hit.gameObject);
            }
        }

        // 進行度（0 → 1）
        float progress = (Time.time - lastAttackTime) / attackInterval;
        progress = Mathf.Clamp01(progress);

        // スケール更新
        if (attackVisual != null)
        {
            float scale = explosionRadius * 2f * progress;
            attackVisual.localScale = new Vector3(scale, scale, 1f);
        }

        // 攻撃
        if (currentTargets.Count > 0)
        {
            if (progress >= 1f)
            {
                Debug.Log("攻撃");
                Attack(currentTargets);
                lastAttackTime = Time.time;
            }
        }
        else
        {
            lastAttackTime = Time.time;

            // リセット（0に戻す）
            if (attackVisual != null)
            {
                attackVisual.localScale = Vector3.zero;
            }
        }
    }

    //  外に出す
    void Attack(HashSet<GameObject> targets)
    {
        foreach (GameObject obj in targets)
        {
            TargetHP hp = obj.GetComponent<TargetHP>();
            if (hp != null)
            {
                hp.TakeDamage(attackPower);
            }
        }

        Debug.Log(targets.Count + " 個のTargetにダメージ");
    }

    //  外に出す
    void OnDrawGizmos()
    {
        if (Pointer.current == null || Camera.main == null) return;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Pointer.current.position.ReadValue());
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(mousePos, explosionRadius);
    }
}
