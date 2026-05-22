using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillButton : MonoBehaviour
{
    //割り当てたスキル(.asset)
    public SkillData skill;
    private SkillManage skillManage;

    void Awake()
    {
        //マネージャの割り当て
        //skillManage = FindObjectByType<SkillManage>(); <古いらしい
        skillManage = FindAnyObjectByType<SkillManage>();
    }

    void Update()
    {
        //componentの変更
        //解放済みなら
        if (skill == null)
        {
            GetComponent<Button>().enabled = true;
            GetComponent<Image>().color = Color.black;
            return;
        }
        if (skillManage.isUnlocked(skill))
        {
            GetComponent<Button>().enabled = false;
            GetComponent<Image>().color = Color.white;
        }
        //解放可能なら
        else if (skillManage.canUnlock(skill))
        {
            GetComponent<Button>().enabled = true;
            GetComponent<Image>().color = Color.orange;
        }
        //解放できないなら
        else
        {
            GetComponent<Button>().enabled = true;
            GetComponent<Image>().color = Color.black;
        }
    }

    //クリック時の処理
    public void OnClick()
    {
        //マネージャ側で処理
        skillManage.getSkill(skill);
    }
}
