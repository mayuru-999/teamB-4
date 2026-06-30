using UnityEngine;

public class PlanetCountManager : MonoBehaviour
{
    public static int planetCount;

    void Awake()
    {
        planetCount = PlayerPrefs.GetInt("PlanetCount", 0);
    }

    public static void AddCount(int amount = 1)
    {
        planetCount += amount;

        PlayerPrefs.SetInt("PlanetCount", planetCount);
        PlayerPrefs.Save();

        Debug.Log("Œ»İ‚Ì”j‰ó”: " + planetCount);
    }

    public static int GetCount()
    {
        return PlayerPrefs.GetInt("PlanetCount", 0);
    }
}