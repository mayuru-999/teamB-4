using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Attack : MonoBehaviour
{
    [Header("攻撃表示")]
    public Transform attackVisual;

    [Header("攻撃設定")]
    public int attackPower = 1;
    public float explosionRadius = 1.0f;
    public float attackInterval = 1.8f;

    private float lastAttackTime = 0f;

    void Update()
    {
        if (Pointer.current == null || Camera.main == null)
            return;

        // マウス座標取得
        Vector2 mousePos =
            Camera.main.ScreenToWorldPoint(
                Pointer.current.position.ReadValue());

        // 攻撃範囲表示をマウス追従
        if (attackVisual != null)
        {
            attackVisual.position = mousePos;
        }

        // 範囲内のCollider取得
        Collider2D[] hitColliders =
            Physics2D.OverlapCircleAll(mousePos, explosionRadius);

        HashSet<GameObject> currentTargets =
            new HashSet<GameObject>();

        foreach (Collider2D hit in hitColliders)
        {
            if (hit.CompareTag("Target"))
            {
                currentTargets.Add(hit.gameObject);
            }
        }

        // 攻撃チャージ進行度
        float progress =
            (Time.time - lastAttackTime) / attackInterval;

        progress = Mathf.Clamp01(progress);

        // エフェクトサイズ更新
        if (attackVisual != null)
        {
            float scale = explosionRadius * 2f * progress;

            attackVisual.localScale =
                new Vector3(scale, scale, 1f);
        }

        // Targetがいる場合
        if (currentTargets.Count > 0)
        {
            // チャージ完了で攻撃
            if (progress >= 1f)
            {
                AttackTargets(currentTargets);

                lastAttackTime = Time.time;
            }
        }
        else
        {
            // Targetがいなければリセット
            lastAttackTime = Time.time;

            if (attackVisual != null)
            {
                attackVisual.localScale = Vector3.zero;
            }
        }
    }

    // ダメージ処理
    void AttackTargets(HashSet<GameObject> targets)
    {
        Debug.Log("攻撃");

        foreach (GameObject obj in targets)
        {
            HPmanager hp = obj.GetComponent<HPmanager>();

            if (hp != null)
            {
                hp.TakeDamage(attackPower);
            }
        }

        Debug.Log(targets.Count + " 個のTargetにダメージ");
    }

    // 攻撃範囲可視化
    void OnDrawGizmos()
    {
        if (Pointer.current == null || Camera.main == null)
            return;

        Vector2 mousePos =
            Camera.main.ScreenToWorldPoint(
                Pointer.current.position.ReadValue());

        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(
            mousePos,
            explosionRadius
        );
    }
}