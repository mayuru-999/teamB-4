using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class WhiteFadeManager : MonoBehaviour
{
    public Image fadeImage;
    public float fadeSpeed = 1.5f;
    public string nextSceneName;

    public void StartFade()
    {
        StartCoroutine(FadeRoutine());
    }

    IEnumerator FadeRoutine()
    {
        float alpha = 0f;

        while (alpha < 1f)
        {
            alpha += Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }

        // ƒV[ƒ“ˆÚ“®
        SceneManager.LoadScene(nextSceneName);
    }
}