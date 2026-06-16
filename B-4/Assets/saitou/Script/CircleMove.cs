using UnityEngine;

public class CircleMove : MonoBehaviour
{
    public RectTransform center;
    public RectTransform image;

    private float angle = 0f;
    private float radius;

    void Start()
    {
        // center ‚©‚ç image ‚Ü‚Å‚Ì‹——£‚ð”¼Œa‚É‚·‚é
        radius = Vector2.Distance(center.anchoredPosition, image.anchoredPosition);
    }

    void Update()
    {
        angle += Time.deltaTime;

        float x = Mathf.Cos(angle) * radius;
        float y = Mathf.Sin(angle) * radius;

        image.anchoredPosition =
            center.anchoredPosition + new Vector2(x, y);
    }
}