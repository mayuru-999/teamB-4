using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillButton : MonoBehaviour
{
    public SkillData skill;
    public TMP_Text skillText;

    private SkillManage skillManage;

    void Start()
    {
        skillManage = FindObjectOfType<SkillManage>();
        skillText.text = skill.skillName;
    }

    public void OnClick()
    {
        Debug.Log("pushed");
        skillManage.getSkill(skill);
    }
}
