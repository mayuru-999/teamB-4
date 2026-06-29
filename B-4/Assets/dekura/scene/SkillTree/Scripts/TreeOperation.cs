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
    [SerializeField] private RectTransform Forcus;
    [SerializeField] private ScrollRect scrollView;
    [SerializeField] private Image orion;

    [Header("Ui")]
    [SerializeField] private TextMeshProUGUI descriptText;
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private TextMeshProUGUI skillPoints;
    [SerializeField] private TextMeshProUGUI needSkillPoints;
    [SerializeField] private Image skillImage;
    [SerializeField] private SkillSprite[] skillImages;

    [Header("Zoom")]
    [SerializeField] private float zoomMin = 0.4f;
    [SerializeField] private float zoomMax = 2.0f;
    [SerializeField] private float zoomSpeed = 0.1f;

    //初期カラー
    [System.NonSerialized] public Color defaultDiscColor;
    [System.NonSerialized] public Color defaultInfoColor;

    //スキルツリー完成フラグ
    [System.NonSerialized] public bool isCompleted = false;
    [System.NonSerialized] public bool mooving = false;

    //拡大率
    private float currentZoom = 1.0f;
    //スキルの表示位置
    private Vector2 currentPosition = new Vector2(523, 0);
    private Vector2 completePosition = new Vector2(523, 0);
    //選択中スキルボタン
    private SkillButton tg = null;

    private void Awake()
    {
        defaultDiscColor = descriptText.color;
        defaultInfoColor = infoText.color;
    }

    public void TreeZoom(PointerEventData eventData)
    {
        if (isCompleted) return;
        float scroll = eventData.scrollDelta.y;
        currentZoom = Mathf.Clamp(currentZoom + scroll * zoomSpeed, zoomMin, zoomMax);
        content.localScale = Vector3.one * currentZoom;

        content.DOComplete();
        CenterOnSkill();
    }
    public void CanselDOAnchorPos()
    {
        if(mooving) return;
        Forcus.DOComplete();
        content.DOComplete();
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
        UpdateUi();
        //センタリング処理
        Debug.Log($"Centering on:{tg.skill}");
        Vector2 tgPos = (Vector2)content.InverseTransformPoint(tg.GetComponent<RectTransform>().position);
        //Dotweenアセット使用、動きがなめらか
        Forcus.DOAnchorPos(tgPos, 0.1f).SetEase(Ease.OutQuart);
        content.DOAnchorPos(-tgPos * currentZoom + currentPosition, 0.8f).SetEase(Ease.OutQuart)
            .OnComplete(() =>
            {
                mooving = false;
            });
    }

    public void TreeCompleted()
    {
        if(isCompleted) return;

        scrollView.movementType = ScrollRect.MovementType.Unrestricted;
        scrollView.horizontal = false;
        scrollView.vertical = false;
        scrollView.inertia = true;

        isCompleted = true;
        UpdateUi();
        orion.DOFade(0.15f, 4.0f);
        content.DOScale(zoomMin, 3.0f).SetEase(Ease.OutQuart);
        Forcus.DOAnchorPos(new Vector2(-362, -1041), 0.1f).SetEase(Ease.OutQuart);
        content.DOAnchorPos(completePosition, 5.0f).SetEase(Ease.OutQuart)
            .OnComplete(() =>
            {
                Debug.Log("Skill Tree Completed!");
                content.DOAnchorPosY(content.anchoredPosition.y + 20f, 2.5f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
            });
    }

    public void ChangeDescription(string text)
    {
        descriptText.text = text;
    }
    public void ChangeInformation(string text, Color color)
    {
        infoText.text = text;
        infoText.color = color;
    }
    public void HideNeedSkillPoints() => needSkillPoints.text = "";
        

    public void UpdateUi()
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

        skillPoints.text = $"鉱石：{SkillPointManager.Instance.skillPoint.ToString()}";

        needSkillPoints.text = isCompleted ? "" : $"必要鉱石：{tg.needPoint}";
        descriptText.text = isCompleted ?  "Congratulations!" : tg.skill.skillDescription;
        infoText.text = isCompleted ? "" : "クリックして解放";

        descriptText.color = defaultDiscColor;
        infoText.color = defaultInfoColor;

        //infoText.color = new Color(204f, 204f, 204f);
    }
}
