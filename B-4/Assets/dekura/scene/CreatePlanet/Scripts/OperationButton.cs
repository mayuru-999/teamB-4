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
        SceneManager.LoadScene("souma.sence");
    }

    public void ToTreeButton()
    {
        SceneManager.LoadScene("SkillTree");
    }
}
