using UnityEngine;
using UnityEngine.InputSystem; // 新しいInput Systemを使う場合

public class TargetClearer : MonoBehaviour
{
    public float explosionRadius = 3.0f; // 消去する範囲の半径

    void Update()
    {
        // マウス左クリック判定（新しいInput System用）
        if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
        {
            ClearArea();
        }
    }

    void ClearArea()
    {
        // 1. マウスの位置をワールド座標に変換
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Pointer.current.position.ReadValue());

        // 2. 指定した円の範囲内にある「すべての」コライダーを取得
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(mousePos, explosionRadius);

        // 3. 見つかったコライダーを順番に処理
        foreach (Collider2D hit in hitColliders)
        {
            // タグがTargetなら消す
            if (hit.CompareTag("Target"))
            {
                Destroy(hit.gameObject);
            }
        }

        Debug.Log(hitColliders.Length + " 個の対象を範囲内で検知しました");
    }

    // 範囲を可視化（Scene画面で青い円が表示されます）
    void OnDrawGizmos()
    {
        if (Pointer.current == null) return;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Pointer.current.position.ReadValue());
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(mousePos, explosionRadius);
    }
}