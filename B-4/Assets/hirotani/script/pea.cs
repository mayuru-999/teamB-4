using UnityEngine;

public class pea : MonoBehaviour
{
    public int damage = 10;

    public GameObject myPrefab;

    void OnDestroy()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj != gameObject && obj.name.Contains(myPrefab.name))
            {
                Debug.Log(obj.name + " にダメージ");
            }
        }
    }


    public void TakeDamage(int dmg)
    {
        Debug.Log(gameObject.name + " がダメージを受けた: " + dmg);

        // 例：HPがあるなら減らす
        // hp -= dmg;

        // hp <= 0なら破壊
        // Destroy(gameObject);
    }
}

