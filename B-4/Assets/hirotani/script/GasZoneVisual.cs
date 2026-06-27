using System.Collections;
using UnityEngine;

public class GasZoneVisual : MonoBehaviour
{
    public float radius = 4f;
    public float fadeTime = 0.5f;

    private Vector3 originalScale;

    void Start()
    {
        originalScale = new Vector3(radius * 2, radius * 2, 1);
        transform.localScale = originalScale;
    }

    public void PlayDestroyEffect()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        float t = 0f;

        while (t < fadeTime)
        {
            t += Time.deltaTime;

            float progress = t / fadeTime;

            transform.localScale = Vector3.Lerp(
                originalScale,
                Vector3.zero,
                progress
            );

            yield return null;
        }

        Destroy(gameObject);
    }
}
