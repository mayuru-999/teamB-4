
using UnityEngine;

public class SpecialDropMove : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Vector3 targetPosition;

    void Start()
    {
        // カメラの右上のワールド座標を取得
        Vector3 screenTopRight = new Vector3(Screen.width, Screen.height, 10f);
        targetPosition = Camera.main.ScreenToWorldPoint(screenTopRight);
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        // 目的地に到達したら消す（必要なら）
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            Destroy(gameObject);
        }
    }
}

