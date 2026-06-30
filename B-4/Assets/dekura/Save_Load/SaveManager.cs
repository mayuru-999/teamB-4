using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    [System.Serializable]
    private class SaveData
    {
        public int gameLv;
        public int bigbang;
        public List<SkillData> unlockedSkills;
        public List<SkillData> unlockedSpSkills;

        public List<string> eventFlagKeys;
        public List<bool> eventFlagValues;

        public int skillPoint;
        public int stardustPoint;
    }

    private string saveDataKey = "SAVEDATA";
    private bool hasSaveData;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        hasSaveData = PlayerPrefs.HasKey(saveDataKey);
        Debug.Log($"SAVEDATA:{hasSaveData}");
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    public void Save()
    {
        var data = new SaveData
        {
            gameLv = SkillManage.Instance.gameLv,
            bigbang = SkillManage.Instance.bigbang,
            unlockedSkills = SkillManage.Instance.unlockedSkills,
            unlockedSpSkills = SkillManage.Instance.unlockedSpSkills,
            skillPoint = SkillPointManager.Instance.skillPoint,
            stardustPoint = SkillPointManager.Instance.starDustPoint,

            eventFlagKeys = SkillManage.Instance.firstEventFlags.Keys.ToList(),
            eventFlagValues = SkillManage.Instance.firstEventFlags.Values.ToList()
        };

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(saveDataKey, json);
        PlayerPrefs.Save();
    }
    public void Load()
    {
        if (!PlayerPrefs.HasKey(saveDataKey))
        {
            Debug.Log("SAVEDATA is NotFound");
            return;
        }

        string json = PlayerPrefs.GetString(saveDataKey);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        SkillManage.Instance.gameLv = data.gameLv;
        SkillManage.Instance.bigbang = data.bigbang;
        SkillManage.Instance.unlockedSkills = data.unlockedSkills;
        SkillManage.Instance.unlockedSpSkills = data.unlockedSpSkills;
        SkillPointManager.Instance.skillPoint = data.skillPoint;
        SkillPointManager.Instance.starDustPoint = data.stardustPoint;

        for (int i = 0; i < data.eventFlagKeys.Count; i++)
        {
            SkillManage.Instance.firstEventFlags[data.eventFlagKeys[i]] = data.eventFlagValues[i];
        }
    }

    public void DeleteSaveData()
    {
        PlayerPrefs.DeleteKey(saveDataKey);
        Debug.Log("SaveData deleated");
    }
}
