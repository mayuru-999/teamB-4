using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ToClearButton : MonoBehaviour
{
    private Image image;
    private Button button;
    private bool active;

    void Start()
    {
        image = gameObject.GetComponent<Image>();
        button = gameObject.GetComponent<Button>();

        image.color = Color.clear;
        button.enabled = false;
        active = false;
    }
    void Update()
    {
        if (active) image.color = Color.HSVToRGB(Time.time % 1, 1, 0.9f);
        else if (SkillManage.Instance.getEffect(SkillEffect.Type.GOD) > 0f) ButtonActivate();
    }

    private void ButtonActivate()
    {
        Debug.Log("Activate");
        active = true;
        image.DOFade(100f, 2f)
            .OnComplete(() =>
            {
                button.enabled = true;
            });
    }

    public void OnClick()
    {
        SceneManager.LoadScene("ClearScene");
    }
}
