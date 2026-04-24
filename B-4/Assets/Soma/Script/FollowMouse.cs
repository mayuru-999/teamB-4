using UnityEngine;
using UnityEngine.InputSystem;

public class FollowMouse : MonoBehaviour
{
    public float radius = 3.0f;

    void Start()
    {
        // Spriteのサイズを爆発半径に合わせる
        // 本来のCircleの直径が1ユニットなので、半径×2をスケールに代入
        transform.localScale = new Vector3(radius * 2, radius * 2, 1);
    }

    void Update()
    {
        // マウスの位置をワールド座標に変換して追従
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Pointer.current.position.ReadValue());
        transform.position = mousePos;
    }
}