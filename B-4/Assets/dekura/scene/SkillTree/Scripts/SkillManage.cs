using System.Collections.Generic;
using System.Drawing;
using UnityEditor.PackageManager;
using UnityEngine;

public class SkillManage : MonoBehaviour
{
    public static SkillManage Instance { get; private set; }
    private TreeOperation treeOperation;
    private SkillButton[] skillButtons;

    //ゲーム全体の段階(Lv）
    private int gameLv = 1;
    //既に解放したスキル情報を入れるリスト
    private List<SkillData> unlockedSkills = new List<SkillData>();

    //PlaneSizeの値をここに記入
    private Vector3[] PlaneSize = new Vector3[]
    {
        new Vector3 (1.0f, 0.0f, 0.0f),
        new Vector3 (0.7f, 0.3f, 0.0f),
        new Vector3 (0.4f, 0.3f, 0.3f),
    };

    //--以下、ゲームLvに応じた、各大きさの惑星ステータス--//
    //例)
    //ゲームLv1(大きさ1,大きさ2,大きさ3)
    //ゲームLv2(大きさ1,大きさ2,大きさ3)...

    //惑星の体力
    private Vector3[] PlaneHealth = new Vector3[]
    {
        new Vector3 (3, 6, 9),
        new Vector3 (13, 16, 19),
        new Vector3 (23, 26, 29),
        new Vector3 (33, 36, 39),
        new Vector3 (43, 46, 49),
    };

    //惑星の量
    private Vector3[] Crystalvol = new Vector3[]
    {
        new Vector3 (1, 3, 5),
        new Vector3 (10, 12, 15),
        new Vector3 (20, 22, 25),
        new Vector3 (30, 32, 35),
        new Vector3 (40, 42, 45),
    };

    //惑星のダスト泥率
    //vector3にする必要はない、後から大きさごとに変えれるように一応vec3
    private Vector3[] StarDastsPar = new Vector3[]
    {
        new Vector3 (5, 5, 5),
        new Vector3 (6, 6, 6),
        new Vector3 (8, 8, 8),
        new Vector3 (9, 9, 9),
        new Vector3 (10, 10, 10),
    };

    //---------------------------------------------------//

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        treeOperation = FindAnyObjectByType<TreeOperation>();
        skillButtons = FindObjectsByType<SkillButton>(FindObjectsSortMode.None);
    }

    //スキル取得時の処理
    public void getSkill(SkillData skill)
    {
        treeOperation = FindAnyObjectByType<TreeOperation>();
        skillButtons = FindObjectsByType<SkillButton>(FindObjectsSortMode.None);

        if (SkillPointManager.Instance.skillPoint < skill.needPoint)
        {
            Debug.Log($"ポイントが足りません。:{SkillPointManager.Instance.skillPoint - skill.needPoint}");
            return;
        }

        //解放処理
        unlockedSkills.Add(skill);
        SkillPointManager.Instance.skillPoint -= skill.needPoint;
        Debug.Log($"解放しました。：{skill}");
        Debug.Log($"スキルポイント：{SkillPointManager.Instance.skillPoint}");

        //UIの更新
        if (treeOperation != null) treeOperation.CenterOnSkill();
        foreach (SkillButton button in skillButtons) button.ButtonUpdate();
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
    public bool canUnlock(SkillData needSkills)
    {
        if (isUnlocked(needSkills)|| needSkills == null)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 同じtypeのスキルの効果量を合計して返す
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public float getEffect(SkillEffect.Type type)
    {
        float effectValue = 0;
        foreach (SkillData skill in unlockedSkills)
        {
            if (skill.effect.type == type)
            {
                if (type == SkillEffect.Type.PlaneLv)
                {
                    effectValue = Mathf.Max(skill.effect.value, effectValue);
                }
                else
                {
                    effectValue += skill.effect.value;
                }
            }
        }
        if (type == SkillEffect.Type.PlaneLv) 
            return effectValue = Mathf.Max(effectValue, 1);
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
        foreach (SkillButton button in skillButtons) button.ButtonUpdate();
    }

    // データだけクリア
    public void ClearSkillData()
    {
        unlockedSkills.Clear();
    }

    /// <summary>
    /// ゲームLvを上げる関数
    /// </summary>
    public void LvUpdate()
    {
        if (gameLv <= 5)
        {
            gameLv++;
            Debug.Log($"Level Up!：{gameLv}");
        }
        else Debug.LogWarning("Warning: Level Max!");
    }

    /// <summary>
    /// ゲームLvに応じた、各大きさの惑星ステータスを返す
    /// </summary>
    /// <returns></returns>
    public (Vector3, Vector3, Vector3) LvtoPlaneData() //タプルっていうらしい
    {
        //toそうま先輩
        //var(health,size,dastpar)=LvtoPlaneData();
        //みたいな感じで受け取れます。
        return (PlaneHealth[gameLv - 1], Crystalvol[gameLv - 1], StarDastsPar[gameLv - 1]);  
    }
}
