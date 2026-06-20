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
    }

    //ホイールスクロールでズームイン、アウト
    public void OnScroll(PointerEventData eventData)
    {
        if(gameObject.name == "Orion") treeOperation.TreeZoom(eventData);
    }

    //クリックでスキルのセンタリング
    //Downも宣言しないとUpが反応しないため、両方宣言
    public void OnPointerDown(PointerEventData eventData) { }
    public void OnPointerUp(PointerEventData eventData)
    {
        if(gameObject.name == "Orion") treeOperation.CenterOnSkill();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (gameObject.name == "ToMainButton") treeOperation.ChangeDescription("メインゲームへ");
        if (gameObject.name == "ToPlaneButton") treeOperation.ChangeDescription("惑星作成へ");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        treeOperation.ResetDescription();
    }

    public void ToMainButton()
    {
        SceneManager.LoadScene("souma.sence");
    }

    public void ToPlaneButton()
    {
    }
}
