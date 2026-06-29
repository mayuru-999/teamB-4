using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager;
using UnityEngine;

public class SkillManage : MonoBehaviour
{
    public static SkillManage Instance { get; private set; }

    private TreeOperation treeOperation;
    private PlanetUiManager planetUiManager;
    private SkillButton[] skillButtons;
    private PlaneSkill[] planeSkill;

    // --- 【追加】画面遷移カウント用の変数 ---
    public int MainVisitCount { get; private set; } = 0; // 他のスクリプトから「SkillManage.Instance.MainVisitCount」で参照可能
    private bool isComingFromSkillTree = false;         // スキルツリーから戻ってきたかどうかの内部フラグ

    // ゲーム全体の段階（Lv）
    [System.NonSerialized] public int gameLv = 1;
    [System.NonSerialized] public int bigbang = 0;

    // 初期ステータス
    [System.NonSerialized] public float baseAttack = 0f;
    [System.NonSerialized] public float baseAttackInterval = 0f;
    [System.NonSerialized] public float baseAttackRange = 0f;
    [System.NonSerialized] public float basePlaneVol = 20f;

    // 既に取得しているスキルを管理するリスト
    private List<SkillData> unlockedSkills = new List<SkillData>();
    private List<SkillData> unlockedSpSkills = new List<SkillData>();

    // PlaneSizeの値をレベル別に設定し、出現率
    private Vector3[] PlaneSize = new Vector3[]
    {
        new Vector3 (1.0f, 0.0f, 0.0f),
        new Vector3 (0.7f, 0.3f, 0.0f),
        new Vector3 (0.4f, 0.3f, 0.3f),
    };

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

        RefreshReferences();
    }

    /// <summary>
    /// シーン内のUIコンポーネントなどの参照を再取得する（追加）
    /// </summary>
    public void RefreshReferences()
    {
        treeOperation = FindAnyObjectByType<TreeOperation>();
        planetUiManager = FindAnyObjectByType<PlanetUiManager>();
        skillButtons = FindObjectsByType<SkillButton>(FindObjectsSortMode.None);
        planeSkill = FindObjectsByType<PlaneSkill>(FindObjectsSortMode.None);
    }

    // --- 【追加】カウント制御用の関数群 ---

    /// <summary>
    /// スキルツリーからメインに戻る直前に呼び出す
    /// </summary>
    public void SetReturnFromSkillTreeFlag()
    {
        isComingFromSkillTree = true;
    }

    /// <summary>
    /// メイン画面がロードされた直後に呼び出して、必要ならカウントを進める
    /// </summary>
    public void CheckAndIncrementVisitCount()
    {
        if (isComingFromSkillTree)
        {
            MainVisitCount++;
            Debug.Log($"【カウント】スキルツリーからメインに戻りました。回数: {MainVisitCount}回");

            // カウントを永続保存したい場合はPlayerPrefsを併用（アプリを落としても覚えている）
            // PlayerPrefs.SetInt("MainVisitCount_Saved", MainVisitCount);

            isComingFromSkillTree = false; // フラグをリセット
        }
    }

    // スキル取得処理
    public bool getSkill(SkillData skill)
    {
        RefreshReferences();

        if (SkillPointManager.Instance.skillPoint < skill.needPoint)
        {
            Debug.Log($"ポイントが足りません: {SkillPointManager.Instance.skillPoint - skill.needPoint}");
            if (treeOperation != null) treeOperation.ChangeInformation("ポイントが足りません!", Color.red);
            SoundsManager.Instance.PlaySound("failure");
            return false;
        }

        unlockedSkills.Add(skill);
        SkillPointManager.Instance.skillPoint -= skill.needPoint;

        Debug.Log($"取得しました: {skill}");
        Debug.Log($"スキルポイント: {SkillPointManager.Instance.skillPoint}");

        SoundsManager.Instance.PlaySound("success");

        if (treeOperation != null)
        {
            treeOperation.mooving = true;
            treeOperation.CenterOnSkill();
        }
        foreach (SkillButton button in skillButtons)
            button.ButtonUpdate();

        return true;
    }

    // Spスキル取得処理
    public bool getSpSkill(SkillData skill)
    {
        RefreshReferences();

        if (SkillPointManager.Instance.starDustPoint < skill.needPoint)
        {
            Debug.Log($"ポイントが足りません: {SkillPointManager.Instance.starDustPoint - skill.needPoint}");
            if (planetUiManager != null) planetUiManager.informationText("ポイントが足りません!", planetUiManager.cautionInfoColor);
            SoundsManager.Instance.PlaySound("failure");
            return false;
        }

        unlockedSpSkills.Add(skill);
        SkillPointManager.Instance.starDustPoint -= skill.needPoint;

        Debug.Log($"取得しました: {skill}");
        Debug.Log($"星のかけら: {SkillPointManager.Instance.starDustPoint}");

        SoundsManager.Instance.PlaySound("success");

        if (planetUiManager != null) planetUiManager.informationText("爆誕!!", planetUiManager.defaultInfoColor);
        if (planetUiManager != null) planetUiManager.UpdateUI();

        foreach (PlaneSkill plane in planeSkill)
            plane.PlaneUpdate();

        return true;
    }

    public bool isUnlocked(SkillData skill)
    {
        if (unlockedSkills.Contains(skill) || skill == null || unlockedSpSkills.Contains(skill))
        {
            return true;
        }
        return false;
    }

    public bool canUnlock(SkillData needSkills)
    {
        if (isUnlocked(needSkills) || needSkills == null)
        {
            return true;
        }
        return false;
    }

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
        foreach (SkillData skill in unlockedSpSkills)
        {
            if (skill.effect.type == type)
            {
                effectValue += skill.effect.value;
            }
        }

        if (type == SkillEffect.Type.PlaneLv)
            return effectValue = Mathf.Max(effectValue, 1);

        return effectValue;
    }

    //planeSizeのlvに応じて出現率を返す
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
        if (getEffect(SkillEffect.Type.GOD) == 0)
        {
            unlockedSkills.Clear();
            foreach (SkillButton button in skillButtons)
                button.ButtonUpdate();
        }
    }

    public void ClearSkillData()
    {
        if (getEffect(SkillEffect.Type.GOD) == 0) unlockedSkills.Clear();
    }

    //ClearSceneで使うリセット用関数
    public void AllClearSkillData()
    {
        unlockedSkills.Clear();
        unlockedSpSkills.Clear();
        gameLv = 1;
        bigbang = 0;
    }

    public void LvUpdate()
    {
        bigbang++;
        Debug.Log($"現在のビッグバンカウント: {bigbang}");
        switch (bigbang)
        {
            case 4:
                gameLv++;
                Debug.Log($"Level Up: {gameLv}");
                break;
            case 8:
                gameLv++;
                Debug.Log($"Level Up: {gameLv}");
                break;
            case 11:
                gameLv++;
                Debug.Log($"Level Up: {gameLv}");
                break;
            case 15:
                gameLv++;
                Debug.Log($"Level Up: {gameLv}");
                break;
            default:
                break;
        }
        if (gameLv > 5)
        {
            gameLv = 5;
            Debug.LogWarning("Warning: Level Max!");
        }
    }

    public (Vector3, Vector3, Vector3) LvtoPlaneData()
    {
        return (
            PlaneHealth[gameLv - 1],
            Crystalvol[gameLv - 1] * getEffect(SkillEffect.Type.CrystalVolumeRate),
            StarDastsPar[gameLv - 1]
        );
    }

    public void ResetVisitCount()
    {
        MainVisitCount = 0; // クラス内部からなら書き換えが可能
        Debug.Log("SkillManage: カウントをリセットしました。");
    }

    //アプリ終了時にsave処理
    private void OnApplicationQuit()
    {

    }
}