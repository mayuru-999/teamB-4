using UnityEngine;

public class DropPointManager : MonoBehaviour
{
    public static int dropPoint;

    void Awake()
    {
        dropPoint = PlayerPrefs.GetInt("DropPoint", 0);
    }

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