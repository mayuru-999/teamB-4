using UnityEngine;
using System.Collections.Generic;

public class SkillManage : MonoBehaviour
{
    public static SkillManage Instance { get; private set; }

    //既に解放したスキル情報を入れるリスト
    private List<SkillData> unlockedSkills = new List<SkillData>();

    //PlaneSizeの値をここに記入
    private Vector3[] PlaneSize = new Vector3[]
    {
        new Vector3 (1.0f, 0.0f, 0.0f),
        new Vector3 (0.7f, 0.3f, 0.0f),
        new Vector3 (0.4f, 0.3f, 0.3f),
    };

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    //スキル取得時の処理
    public void getSkill(SkillData skill)
    {
        //必要なスキルが解放されていないならreturn
        if (!canUnlock(skill))
        {
            Debug.Log("まだ解放できません。");
            return;
        }
        if (SkillPointManager.Instance.skillPoint < skill.needPoint)
        {
            Debug.Log("ポイントが足りません。");
            return;
        }

        Debug.Log("解放しました。");
        unlockedSkills.Add(skill);
        SkillPointManager.Instance.skillPoint -= skill.needPoint;
    }

    /// <summary>
    /// すでに開放済みか
    /// </summary>
    /// <param name="skill"></param>
    /// <returns></returns>
    public bool isUnlocked(SkillData skill)
    {
        if (unlockedSkills.Contains(skill) || skill == null)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// スキルを解放可能か返す
    /// </summary>
    /// <param name="skill"></param>
    /// <returns></returns>
    public bool canUnlock(SkillData skill)
    {
        if (skill.needSkillData.Count == 0)
        {
            return true;
        }
        foreach (SkillData needSkill in skill.needSkillData)
        {
            if (!isUnlocked(needSkill))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 同じtypeのスキルの効果量を合計して返す
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public float getEffect(SkillEffect.Type type)
    {
        if (type == null || type == SkillEffect.Type.PlaneLv) return 0;

        float effectValue = 0;
        foreach (SkillData skill in unlockedSkills)
        {
            if (skill.effect.type == type)
            {
                effectValue += skill.effect.value;
            }
        }
        //Debug.Log($"{type}の効果量:{effectValue}");
        return effectValue;
    }
    
    /// <summary>
    /// PlaneSizeをLvに応じて返す
    /// </summary>
    /// <returns></returns>
    public Vector3 getPlaneSizeLv()
    {
        int planeLv = 1;

        foreach (SkillData skill in unlockedSkills)
        {
            if (skill.effect.type == SkillEffect.Type.PlaneLv)
            {
                planeLv = Mathf.Max((int)skill.effect.value, planeLv);
            }
        }
        return PlaneSize[planeLv - 1];
    }

    public void ResetUnlockedSkill()
    {
        unlockedSkills.Clear();
    }
}
