using UnityEngine;

public class DropPointManager : MonoBehaviour
{
    public static int dropPoint;

    // このオブジェクトが壊れたときにもらえるDP
    public int destroyPoint = 100;

    void Awake()
    {
        dropPoint = PlayerPrefs.GetInt("DropPoint", 0);
    }

    private void OnDestroy()
    {
        AddDP(destroyPoint);
    }

    public static void AddDP(int amount)
    {
        dropPoint += amount;

        PlayerPrefs.SetInt("DropPoint", dropPoint);
        PlayerPrefs.Save();

        Debug.Log("現在のDP: " + dropPoint);
    }

    public static int GetDP()
    {
        return PlayerPrefs.GetInt("DropPoint", 0);
    }
}