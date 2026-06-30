using DG.Tweening;
using System.Collections;
using TMPro;
using Unity.Properties;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlanetUiManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI planetName;
    [SerializeField] private TextMeshProUGUI descriptionTxt;
    [SerializeField] private TextMeshProUGUI stardustPoint;
    [SerializeField] private TextMeshProUGUI infoText;

    [Header("Button")]
    [SerializeField] private Button getPlanetButton;

    [System.NonSerialized] public Color defaultInfoColor;
    [System.NonSerialized] public Color cautionInfoColor;
    private PlanetOperation planetOperation;

    void Start()
    {
        planetOperation = FindAnyObjectByType<PlanetOperation>();
        defaultInfoColor = infoText.color;
        cautionInfoColor = new Color(200f / 255f, 0f / 255f, 0f / 255f);
        infoText.alpha = 0f;
        UpdateUI();
    }

    public void UpdateUI()
    {
        //focus(軌道上一番上)が当たっているオブジェクトのPlaneSkillを取得
        PlaneSkill planeSkill = planetOperation.GetFocusobject().GetComponent<PlaneSkill>();
        Debug.Log($"{planeSkill.skill}");

        planetName.text = "???";
        descriptionTxt.text = $"必要なかけら：{planeSkill.skill.needPoint}";
        stardustPoint.text = $"かけら：{SkillPointManager.Instance.starDustPoint}";

        if (SkillManage.Instance.isUnlocked(planeSkill.skill))
        {
            getPlanetButton.interactable = false;
            planetName.text = planeSkill.planeName;
            descriptionTxt.text = planeSkill.skill.skillDescription;
            getPlanetButton.GetComponentInChildren<TextMeshProUGUI>().text = "取得済み";
        }
        else if (SkillManage.Instance.canUnlock(planeSkill.needSkill))
        {
            getPlanetButton.interactable = true;
            getPlanetButton.GetComponentInChildren<TextMeshProUGUI>().text = "創造する";
        }
        else
        {
            getPlanetButton.interactable = false;
            getPlanetButton.GetComponentInChildren<TextMeshProUGUI>().text = "前の惑星が必要...";
        }
    }

    public void ChangeDescription(string description)
    {
        descriptionTxt.text = description;
    }

    public void informationText(string text, Color color)
    {
        infoText.DOComplete();

        infoText.alpha = 1f;
        infoText.text = text;
        infoText.color = color;

        infoText.DOFade(0f, 1f).SetDelay(2f);
    }
}
