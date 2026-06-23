using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlaneSkill : MonoBehaviour
{
    [SerializeField] public string planeName;
    //割り当てたスキル(.asset)
    [SerializeField] public SkillData skill;
    [SerializeField] public SkillData needSkill;
    [SerializeField] public int needPoint;

    private PlanetOperation planetOperation;

    private Color defaultColor;
    private Sprite defaultSprite;
    private Image shader;

    void Start()
    {
        planetOperation = FindAnyObjectByType<PlanetOperation>();

        defaultColor = GetComponent<Image>().color;
        defaultSprite = GetComponent<Image>().sprite;
        shader = transform.Find("Shader").gameObject.GetComponent<Image>();

        shader.GetComponent<Animator>().enabled = false;

        if (skill != null) skill.needPoint = needPoint;

        PlaneUpdate();
    }

    //クリック時の処理
    public void OnClick()
    {
        //マネージャ側で処理
        SkillManage.Instance.getSpSkill(skill);
    }

    public void PlaneUpdate()
    {
        if (skill == null)
        {
            GetComponent<Image>().color = new Color(0, 0, 0, 0);
            return;
        }
        //解放済みなら
        else if (SkillManage.Instance.isUnlocked(skill))
        {
            GetComponent<Image>().color = defaultColor;
            GetComponent<Image>().sprite = defaultSprite;
            shader.GetComponent<Animator>().enabled = false;
            shader.DOColor(Color.clear, 0.5f).SetEase(Ease.Linear);
        }
        //解放可能なら
        else if (SkillManage.Instance.canUnlock(needSkill))
        {
            shader.sprite = planetOperation.canUnlockPlane;
            shader.GetComponent<Animator>().enabled = true;
        }
        //解放できないなら
        else
        {
            shader.sprite = planetOperation.hidenPlane;
        }

        Debug.Log($"PlaneUpdate: {GetComponent<Image>().color}");
    }
}
