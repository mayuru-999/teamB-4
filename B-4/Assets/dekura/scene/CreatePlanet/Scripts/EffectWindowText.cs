using TMPro;
using UnityEngine;

public class EffectWindowText : MonoBehaviour
{
    private EffectWindowText[] effectWindowText;

    [SerializeField] private SkillEffect.Type type;
    private TextMeshProUGUI text;

    void Start()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();
        UpdateUi();
    }

    public void OnClick()
    {
        effectWindowText = FindObjectsByType<EffectWindowText>(FindObjectsSortMode.None);

        foreach (EffectWindowText EffectWindow in effectWindowText)
            EffectWindow.UpdateUi();
    }

    public void UpdateUi()
    {
        float effect = SkillManage.Instance.getEffect(type);

        switch (type)
        {
            case SkillEffect.Type.Attack:
                effect += SkillManage.Instance.baseAttack;
                break;

            case SkillEffect.Type.Speed:
                text.text = $"-{effect}%";
                return;

            case SkillEffect.Type.Range:
                text.text = $"+{effect}%";
                return;

            case SkillEffect.Type.PlaneVolume:
                effect += SkillManage.Instance.basePlaneVol;
                break;

            case SkillEffect.Type.none:
                return;
        }

        text.text = $"{effect}";
    }
}
