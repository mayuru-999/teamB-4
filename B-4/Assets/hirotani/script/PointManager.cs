using UnityEngine;

public class PointManager : MonoBehaviour
{
    public static int skillPoint;
    public static int dropPoint;

    void Awake()
    {
        skillPoint = PlayerPrefs.GetInt("SkillPoint", 0);
        dropPoint = PlayerPrefs.GetInt("DropPoint", 0);
    }

    // ===== SP =====
    public static void AddSP(int amount)
    {
        skillPoint += amount;

        PlayerPrefs.SetInt("SkillPoint", skillPoint);
        PlayerPrefs.Save();

        Debug.Log("åªç›ÇÃSP: " + skillPoint);
    }

    public static int GetSP()
    {
        return PlayerPrefs.GetInt("SkillPoint", 0);
    }

    // ===== DP =====
    public static void AddDP(int amount)
    {
        dropPoint += amount;

        PlayerPrefs.SetInt("DropPoint", dropPoint);
        PlayerPrefs.Save();

        Debug.Log("åªç›ÇÃDP: " + dropPoint);
    }

    public static int GetDP()
    {
        return PlayerPrefs.GetInt("DropPoint", 0);
    }
}