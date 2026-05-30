using UnityEngine;

public class DeleteByTag : MonoBehaviour
{
    public string targetTag = "Target";
    public int point = 10; // 1体あたりのポイント

    void OnMouseDown()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag(targetTag);

        foreach (GameObject obj in objs)
        {
            SkillPointManager.AddScore(point); // ←ポイント加算
            Destroy(obj);
        }
    }
}