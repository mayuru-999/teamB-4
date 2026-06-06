using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    //割り当てたスキル(.asset)
    [SerializeField] public SkillData skill;
    [SerializeField] public SkillData needSkill;
    [SerializeField] public int needPoint;

    void Start()
    {
        ButtonUpdate();
        skill.needPoint = needPoint;
    }

    //クリック時の処理
    public void OnClick()
    {
        //マネージャ側で処理
        SkillManage.Instance.getSkill(skill);
    }

    //スキルの状態に応じて、ボタンの見た目と有効状態を更新
    public void ButtonUpdate()
    {
        if (skill == null)
        {
            GetComponent<Button>().enabled = false;
            GetComponent<Image>().color = Color.black;
            return;
        }
        //解放済みなら
        else if (SkillManage.Instance.isUnlocked(skill))
        {
            GetComponent<Button>().enabled = false;
            GetComponent<Image>().color = Color.white;
        }
        //解放可能なら
        else if (SkillManage.Instance.canUnlock(needSkill))
        {
            GetComponent<Button>().enabled = true;
            GetComponent<Image>().color = Color.orange;
        }
        //解放できないなら
        else
        {
            GetComponent<Button>().enabled = false;
            GetComponent<Image>().color = Color.black;
        }
    }

    //tree側で、解放可能なスキルを探すための関数
    public SkillButton isUnlockable()
    {
        if (skill == null) return null;

        if (SkillManage.Instance.canUnlock(needSkill)&& !SkillManage.Instance.isUnlocked(skill))
        {
            return this;
        }
        return null;
    }
}
