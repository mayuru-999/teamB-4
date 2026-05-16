using UnityEngine;
using System.Collections;

public class SelectButton : MonoBehaviour
{
    public RectTransform container;
    public float moveDistance = 1000f;
    public float moveSpeed = 8f;

    private int i = 0;

    public void RightClick()
    {
        i++;
        StartCoroutine(MoveTo(-i * moveDistance));
    }
    public void LeftClick()
    {
        if (i <= 0) { return; }
        i--;
        StartCoroutine(MoveTo(-i * moveDistance));
    }

    IEnumerator MoveTo(float tgX)
    {
        Vector2 start = container.anchoredPosition;
        Vector2 tg = new Vector2(tgX, start.y);

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * moveSpeed;

            container.anchoredPosition = Vector2.Lerp(start, tg, t);

            yield return null;
        }
    }
}
