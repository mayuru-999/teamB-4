using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillButton : MonoBehaviour
{
    //割り当てたスキル(.asset)
    public SkillData skill;
    private SkillManage skillManage;

    public GameObject button;

    void Start()
    {
        //マネージャの割り当て
        skillManage = FindObjectOfType<SkillManage>();
    }

    void Update()
    {
        if (skillManage.isUnlocked(skill))
        {
            //button.GetComponent<image>().color = Color.Black;
        }
    }

    //クリック時の処理
    public void OnClick()
    {
        //マネージャ側で処理
        //Debug.Log("pushed");
        skillManage.getSkill(skill);
    }
}
