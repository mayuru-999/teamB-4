using UnityEngine;

public class SPManager : MonoBehaviour
{
    public static int skillPoint;

    void Awake()
    {
        skillPoint = PlayerPrefs.GetInt("SkillPoint", 0);
    }

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
}
