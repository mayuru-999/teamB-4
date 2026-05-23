


using UnityEngine;

public class DropItem : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float spreadDistance = 2f;   // 広がる距離
    public float spreadTime = 0.5f;     // 広がる時間

    private Vector3 targetPosition;
    private Vector3 startPosition;
    private Vector3 spreadTarget;

    private float timer = 0f;
    private bool isSpreading = true;

    public static int destroyedCount = 0;

    void Start()
    {
        // 左上座標
        targetPosition = Camera.main.ViewportToWorldPoint(
            new Vector3(0f, 1f, 10f));
        targetPosition.z = 0f;

        startPosition = transform.position;

        // ランダム方向に広がる
        Vector2 randomDir = Random.insideUnitCircle.normalized;
        spreadTarget = startPosition + (Vector3)(randomDir * spreadDistance);
    }

    void Update()
    {

        // 回転（毎フレーム）
        transform.Rotate(0, 0, 180f * Time.deltaTime);

        if (isSpreading)
        {
            timer += Time.deltaTime;
            float t = timer / spreadTime;

            // イーズアウト（ふわっと減速）
            t = 1f - Mathf.Pow(1f - t, 3f);

            transform.position = Vector3.Lerp(startPosition, spreadTarget, t);

            if (t >= 1f)
            {
                isSpreading = false;
            }
        }
        else
        {
            // 左上へ移動
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                destroyedCount++;
                Debug.Log("消えた数: " + destroyedCount);
                Destroy(gameObject);
            }
        }
    }
}
