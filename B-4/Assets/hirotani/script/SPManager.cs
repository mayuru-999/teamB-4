using UnityEngine;

public class SPManager : MonoBehaviour
{
    public static int skillPoint;

    // このオブジェクトが壊れたときにもらえるSP
    public int destroyPoint = 100;

    void Awake()
    {
        skillPoint = PlayerPrefs.GetInt("SkillPoint", 0);
    }

    private void OnDestroy()
    {
        AddSP(destroyPoint);
    }

    public static void AddSP(int amount)
    {
        skillPoint += amount;

        PlayerPrefs.SetInt("SkillPoint", skillPoint);
        PlayerPrefs.Save();

        Debug.Log("現在のSP: " + skillPoint);
    }

    public static int GetSP()
    {
        return PlayerPrefs.GetInt("SkillPoint", 0);
    }
}
