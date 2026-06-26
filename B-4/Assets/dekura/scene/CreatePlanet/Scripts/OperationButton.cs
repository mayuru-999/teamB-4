using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class OperationButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private PlanetUiManager planetUiManager;
    void Awake()
    {
        planetUiManager = FindAnyObjectByType<PlanetUiManager>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (gameObject.name == "ToMainButton")
        {
            planetUiManager.ChangeDescription("メインゲームへ");
        }

        if (gameObject.name == "ToTreeButton")
        {
            planetUiManager.ChangeDescription("スキルツリーへ");
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        planetUiManager.UpdateUI();
    }

    public void ToMainButton()
    {
        if (SkillManage.Instance != null)
        {
            SkillManage.Instance.SetReturnFromSkillTreeFlag();
        }

        SoundsManager.Instance.PlaySound("select2");
        SceneManager.LoadScene("souma.sence");
    }

    public void ToTreeButton()
    {
        SoundsManager.Instance.PlaySound("select2");
        SceneManager.LoadScene("SkillTree");
    }
}
