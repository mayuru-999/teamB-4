using UnityEngine;

public class DeleteByTag : MonoBehaviour
{
    public string targetTag = "Target";
    public int damage = 10;
    public float attackInterval = 2f;

    private float timer = 0f;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            timer += Time.deltaTime;

            if (timer >= attackInterval)
            {
                AttackAll();
                timer = 0f;
            }
        }
        else
        {
            timer = 0f;
        }
    }

    void AttackAll()
    {
        GameObject[] objs =
            GameObject.FindGameObjectsWithTag(targetTag);

        foreach (GameObject obj in objs)
        {
            HPmanager hp = obj.GetComponent<HPmanager>();

            if (hp != null)
            {
                hp.TakeDamage(damage, 2f);
            }
        }

        Debug.Log("í∑âüÇµçUåÇÅI");

        if (SkillManage.Instance != null)
        {
            SkillManage.Instance.ClearSkillData();
        }
    }
}