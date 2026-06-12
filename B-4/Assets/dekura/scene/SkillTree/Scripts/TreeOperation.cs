using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TreeOperation : MonoBehaviour
{
    [System.Serializable]
    private struct SkillSprite
    {
        public SkillEffect.Type type;
        public Sprite sprite;
    }

    [Header("Orion")]
    [SerializeField] private RectTransform viewport;
    [SerializeField] private RectTransform content;
    [SerializeField] private ScrollRect scrollView;
    [SerializeField] private Image orion;

    [Header("Ui")]
    [SerializeField] private TextMeshProUGUI descriptText;
    [SerializeField] private Image skillImage;
    [SerializeField] private SkillSprite[] skillImages;

    [Header("Zoom")]
    [SerializeField] private float zoomMin = 0.5f;
    [SerializeField] private float zoomMax = 2.0f;
    [SerializeField] private float zoomSpeed = 0.1f;

    //スキルツリー完成フラグ
    protected bool isCompleted = false;
    //拡大率
    private float currentZoom = 1.0f;
    //スキルの表示位置
    private Vector2 currentPosition = new Vector2(-390, 0);
    //選択中スキルボタン
    private SkillButton tg = null;

    public void TreeZoom(PointerEventData eventData)
    {
        if (isCompleted) return;
        float scroll = eventData.scrollDelta.y;
        currentZoom = Mathf.Clamp(currentZoom + scroll * zoomSpeed, zoomMin, zoomMax);
        content.localScale = Vector3.one * currentZoom;
    }

    public void CenterOnSkill()
    {
        if (isCompleted) return;
        //SkillButtonを全て探す。
        //解放可能なスキルがあれば、そのSkillButtonをtgに格納。
        SkillButton[] skillButtons = FindObjectsByType<SkillButton>(FindObjectsSortMode.None);
        tg = null;
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

        //スキルの画像と説明を更新
        ResetDescription();
        //センタリング処理
        Debug.Log($"Centering on:{tg.skill}");
        Vector2 tgPos = (Vector2)content.InverseTransformPoint(tg.GetComponent<RectTransform>().position);
        //Dotweenアセット使用、動きがなめらか
        content.DOAnchorPos(-tgPos * currentZoom + currentPosition, 0.8f).SetEase(Ease.OutQuart);
    }

    public void TreeCompleted()
    {
        if(isCompleted) return;

        scrollView.movementType = ScrollRect.MovementType.Unrestricted;
        scrollView.horizontal = false;
        scrollView.vertical = false;
        scrollView.inertia = true;

        isCompleted = true;
        ResetDescription();
        orion.DOFade(0.15f, 4.0f);
        content.DOScale(zoomMin, 3.0f).SetEase(Ease.OutQuart);
        content.DOAnchorPos(currentPosition, 5.0f).SetEase(Ease.OutQuart).OnComplete(() =>
        {
            Debug.Log("Skill Tree Completed!");
            content.DOAnchorPosY(content.anchoredPosition.y + 20f, 2.5f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        });
    }

    public void ChangeDescription(string text)
    {
        descriptText.text = text;
    }
    public void ResetDescription()
    {
        if(tg != null)
        {
            foreach (var skill in skillImages)
            {
                if (skill.type == tg.skill.effect.type)
                {
                    skillImage.sprite = skill.sprite;
                    break;
                }
            }
        }
        else skillImage.color = Color.clear;
        descriptText.text = isCompleted ?  "Congratulations!" : tg.skill.skillDescription;
    }
}
