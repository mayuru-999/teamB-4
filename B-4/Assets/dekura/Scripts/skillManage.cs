using UnityEngine;
using System.Collections.Generic;

public class SkillManage : MonoBehaviour
{
    //スキル情報(.asset)を入れるリスト
    public List<SkillData> skills;

    //既に解放したスキル情報を入れるリスト
    private List<SkillData> unlockedSkills = new List<SkillData>();

    //スキル取得時の処理
    public void getSkill(SkillData skill)
    {
        //既に開放済みならreturn
        if (unlockedSkills.Contains(skill))
        {
            Debug.Log("解放済みです。");
            return;
        }
        ////スキルIDを参照して解放できるかどうか確認
        //if (!unlockedSkills.Contains(skill.id))
        //{
        //    debug.Log("まだ解放できません。");
        //    return;
        //}
        Debug.Log("解放しました。");
        unlockedSkills.Add(skill);
    }
}
