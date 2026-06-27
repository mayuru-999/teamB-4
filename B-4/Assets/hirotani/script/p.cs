using System.Collections.Generic;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public List<GameObject> list1 = new List<GameObject>();
    public List<GameObject> list2 = new List<GameObject>();

    public int score = 0;

    public int list1Point = 100; // リスト1のポイント
    public int list2Point = 200; // リスト2のポイント

    void Update()
    {
        CheckDestroyed(list1, list1Point);
        CheckDestroyed(list2, list2Point);
    }

    void CheckDestroyed(List<GameObject> list, int point)
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (list[i] == null)
            {
                score += point;
                Debug.Log($"ポイント獲得！ 現在のスコア: {score}");

                list.RemoveAt(i);
            }
        }
    }
}
