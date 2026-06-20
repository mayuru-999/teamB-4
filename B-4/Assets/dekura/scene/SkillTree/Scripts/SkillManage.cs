using System.Collections.Generic;
using UnityEngine;

public class SkillManage : MonoBehaviour
{
    public static SkillManage Instance { get; private set; }
    private TreeOperation treeOperation;
    private SkillButton[] skillButtons;

    // ゲーム全体の段階（Lv）
    private int gameLv = 1;

    // 既に取得しているスキルを管理するリスト
    private List<SkillData> unlockedSkills = new List<SkillData>();

    // PlaneSizeの値をレベル別に設定
    // --- SkillManage.cs の修正部分 ---

    // PlaneSize をゲームレベル5まで対応できるように要素数を増やす
    private Vector3[] PlaneSize = new Vector3[]
    {
        new Vector3 (1.0f, 1.5f, 2.0f), // レベル1
        new Vector3 (1.1f, 1.6f, 2.1f), // レベル2
        new Vector3 (1.2f, 1.7f, 2.2f), // レベル3
        new Vector3 (1.3f, 1.8f, 2.3f), // レベル4
        new Vector3 (1.4f, 1.9f, 2.4f), // レベル5
    };

    // 各レベルごとのステータス
    // 例：
    // Lv1（サイズ1,2,3）
    // Lv2（サイズ1,2,3）...

    // HP
    private Vector3[] PlaneHealth = new Vector3[]
    {
        new Vector3 (3, 6, 9),
        new Vector3 (13, 16, 19),
        new Vector3 (23, 26, 29),
        new Vector3 (33, 36, 39),
        new Vector3 (43, 46, 49),
    };

    // クリスタル量
    private Vector3[] Crystalvol = new Vector3[]
    {
        new Vector3 (1, 3, 5),
        new Vector3 (10, 12, 15),
        new Vector3 (20, 22, 25),
        new Vector3 (30, 32, 35),
        new Vector3 (40, 42, 45),
    };

    // スターダスト量
    private Vector3[] StarDastsPar = new Vector3[]
    {
        new Vector3 (5, 5, 5),
        new Vector3 (6, 6, 6),
        new Vector3 (8, 8, 8),
        new Vector3 (9, 9, 9),
        new Vector3 (10, 10, 10),
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
        treeOperation = FindAnyObjectByType<TreeOperation>();
        skillButtons = FindObjectsByType<SkillButton>(FindObjectsSortMode.None);
    }

    // スキル取得処理
    public void getSkill(SkillData skill)
    {
        treeOperation = FindAnyObjectByType<TreeOperation>();
        skillButtons = FindObjectsByType<SkillButton>(FindObjectsSortMode.None);

        if (SkillPointManager.Instance.skillPoint < skill.needPoint)
        {
            Debug.Log($"ポイントが足りません: {SkillPointManager.Instance.skillPoint - skill.needPoint}");
            return;
        }

        unlockedSkills.Add(skill);
        SkillPointManager.Instance.skillPoint -= skill.needPoint;

        Debug.Log($"取得しました: {skill}");
        Debug.Log($"スキルポイント: {SkillPointManager.Instance.skillPoint}");

        SkillPointManager.Instance.UpdateUI();
        if (treeOperation != null) treeOperation.CenterOnSkill();

        foreach (SkillButton button in skillButtons)
            button.ButtonUpdate();
    }

    // すでに解放済みか
    public bool isUnlocked(SkillData skill)
    {
        return unlockedSkills.Contains(skill) || skill == null;
    }

    // 解放可能か判定
    public bool canUnlock(SkillData needSkills)
    {
        return isUnlocked(needSkills) || needSkills == null;
    }

    // 指定タイプの効果量を合計
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
            return Mathf.Max(effectValue, 1);

        return effectValue;
    }

    // PlaneSizeをLvに応じて取得
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
        foreach (SkillButton button in skillButtons)
            button.ButtonUpdate();
    }

    // データリセット
    public void ClearSkillData()
    {
        unlockedSkills.Clear();
    }

    // ゲームLvを上げる
    public void LvUpdate()
    {
        if (gameLv <= 5)
        {
            gameLv++;
            Debug.Log($"Level Up: {gameLv}");
        }
        else
        {
            Debug.LogWarning("Warning: Level Max");
        }
    }

    // Lvに応じたステータスを返す
    public (Vector3 health, Vector3 size, Vector3 crystal) LvtoPlaneData()
    {
        return (
            PlaneHealth[gameLv - 1],
            PlaneSize[gameLv - 1],
            Crystalvol[gameLv - 1]
        );
    }
}