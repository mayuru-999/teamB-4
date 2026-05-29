using UnityEngine;
using System.Collections.Generic;

public class SkillManage : MonoBehaviour
{
    public static SkillManage Instance { get; private set; }

    //既に解放したスキル情報を入れるリスト
    private List<SkillData> unlockedSkills = new List<SkillData>();

    private Vector3[] PlaneSizeLv = new Vector3[]
    {
        new Vector3 (0.0f, 0.0f, 0.0f),
        new Vector3 (0.0f, 0.0f, 0.0f),
        new Vector3 (0.0f, 0.0f, 0.0f),
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

    //すでに開放済みか
    public bool isUnlocked(SkillData skill)
    {
        if (unlockedSkills.Contains(skill) || skill == null)
        {
            return true;
        }
        return false;
    }

    //解放可能かどうか
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

    //同じtypeのスキルの効果量を合計して返す
    public float getEffect(SkillEffect.Type type)
    {
        float effectValue = 0;
        if (type == SkillEffect.Type.PlaneSize)
        {
            effectValue = 1;
            foreach (SkillData skill in unlockedSkills)
            {
                if (skill.effect.type == SkillEffect.Type.PlaneSize)
                {
                    effectValue = Mathf.Max(skill.effect.value, effectValue);
                }
            }
            return 0;
        }

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

    public void ResetUnlockedSkill()
    {
        unlockedSkills.Clear();
    }
}
