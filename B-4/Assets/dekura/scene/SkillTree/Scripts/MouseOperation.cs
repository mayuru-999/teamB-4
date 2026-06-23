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
            treeOperation.ChangeInformation("クリックして移動", new Color(204f/255f, 204f/255f, 204f/255f));
        }

        if (gameObject.name == "ToPlaneButton")
        {
            treeOperation.ChangeDescription("惑星作成へ");
            treeOperation.ChangeInformation("クリックして移動", new Color(204f/255f, 204f/255f, 204f/255f));
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        treeOperation.UpdateUi();
    }

    public void ToMainButton()
    {
        SceneManager.LoadScene("souma.sence");
    }

    public void ToPlaneButton()
    {
        SceneManager.LoadScene("CreatePlanet");
    }
}
