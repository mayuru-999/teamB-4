using UnityEngine;

public class DeleteByTag : MonoBehaviour
{
    public string targetTag = "Target";

    void OnMouseDown()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag(targetTag);

        foreach (GameObject obj in objs)
        {
            Destroy(obj);
        }
    }
}