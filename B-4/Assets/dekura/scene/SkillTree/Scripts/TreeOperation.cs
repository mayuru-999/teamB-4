using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TreeOperation : MonoBehaviour, IScrollHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform viewport;
    [SerializeField] private RectTransform content;
    [SerializeField] private ScrollRect scrollView;
    [SerializeField] private Image orion;

    [Header("Zoom")]
    [SerializeField] private float zoomMin = 0.5f;
    [SerializeField] private float zoomMax = 2.0f;
    [SerializeField] private float zoomSpeed = 0.1f;

    //スキルツリー完成フラグ
    private bool isCompleted = false;
    //拡大率
    private float currentZoom = 1.0f;
    //スキルの表示位置
    private Vector2 currentPosition = new Vector2(-390, 0);
    private Vector2 pointerDownPosition;

    //ホイールスクロールでズームイン、アウト
    public void OnScroll(PointerEventData eventData)
    {
        if (isCompleted) return;
        float scroll = eventData.scrollDelta.y;
        currentZoom = Mathf.Clamp(currentZoom + scroll * zoomSpeed, zoomMin, zoomMax);
        content.localScale = Vector3.one * currentZoom;
    }

    private void Start()
    {
        //初期位置にセンタリング
        CenterOnSkill();
    }

    //ドラッグ開始
    public void OnPointerDown(PointerEventData eventData)
    {
        //pointerDownPosition = eventData.position;
    }

    //ドラッグ終了
    public void OnPointerUp(PointerEventData eventData)
    {
        //ドラッグしていない場合は、解放可能なスキルを探して、そこにセンタリングする
        //if (eventData.position != pointerDownPosition) return;
        if(isCompleted) return;
        CenterOnSkill();
    }

    public void CenterOnSkill()
    {
        //SkillButtonを全て探す。
        //解放可能なスキルがあれば、そのSkillButtonをtgに格納。
        SkillButton[] skillButtons = FindObjectsByType<SkillButton>(FindObjectsSortMode.None);
        SkillButton tg = null;
        foreach (SkillButton button in skillButtons)
        {
            tg = button.isUnlockable();
            if (tg != null) break;
        }
        //解放可能なスキルがない場合は、慣性スクロールを有効にして終了。
        if (tg == null)
        {
            TreeCompleted();
            return;
        }

        //センタリング処理
        Debug.Log($"Centering on:{tg.skill}");
        Vector2 tgPos = (Vector2)content.InverseTransformPoint(tg.GetComponent<RectTransform>().position);
        //Dotweenアセット使用、動きがなめらか
        content.DOAnchorPos(-tgPos * currentZoom + currentPosition, 0.8f).SetEase(Ease.OutQuart);
    }

    public void TreeCompleted()
    {
        if(isCompleted) return;

        scrollView.horizontal = false;
        scrollView.vertical = false;
        scrollView.inertia = true;
        scrollView.movementType = ScrollRect.MovementType.Unrestricted;
        orion.DOFade(0.15f, 4.0f);
        content.DOScale(zoomMin, 3.0f).SetEase(Ease.OutQuart);
        content.DOAnchorPos(currentPosition, 5.0f).SetEase(Ease.OutQuart).OnComplete(() =>
        {
            isCompleted = true;
            Debug.Log("Skill Tree Completed!");
            content.DOAnchorPosY(content.anchoredPosition.y + 20f, 2.5f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        });
    }
}
