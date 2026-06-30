using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MouseOperation : MonoBehaviour, IScrollHandler, IPointerUpHandler,IPointerDownHandler,IPointerEnterHandler, IPointerExitHandler
{
    private TreeOperation treeOperation;
    void Start()
    {
        treeOperation = FindAnyObjectByType<TreeOperation>();
        treeOperation.CenterOnSkill();
        SkillPointManager.Instance.UpdateUI();
    }

    //ホイールスクロールでズームイン、アウト
    public void OnScroll(PointerEventData eventData)
    {
        if(gameObject.name == "Orion") treeOperation.TreeZoom(eventData);
    }

    //クリックでスキルのセンタリング
    public void OnPointerDown(PointerEventData eventData)
    {
        treeOperation.CanselDOAnchorPos();
        SoundsManager.Instance.PlaySound("select");
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if(gameObject.name == "Orion") treeOperation.CenterOnSkill();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (gameObject.name == "ToMainButton")
        {
            treeOperation.ChangeDescription("メインゲームへ");
            treeOperation.ChangeInformation("クリックして移動", treeOperation.defaultInfoColor);
            treeOperation.HideNeedSkillPoints();
        }

        if (gameObject.name == "ToPlaneButton" && SkillManage.Instance.GetFlags("firstCreateUnlocked"))
        {
            treeOperation.ChangeDescription("惑星作成へ");
            treeOperation.ChangeInformation("クリックして移動", treeOperation.defaultInfoColor);
            treeOperation.HideNeedSkillPoints();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        treeOperation.UpdateUi();
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

    public void ToPlaneButton()
    {
        SoundsManager.Instance.PlaySound("select2");
        SceneManager.LoadScene("CreatePlanet");
    }
}
