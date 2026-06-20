using UnityEngine;

public class DeleteByTag : MonoBehaviour
{
    public WhiteFadeManager fadeManager;
    public string targetTag = "Target";
    public int damage = 10;
    public float attackInterval = 1f;

    private float timer = 0f;

    // 追加
    private bool isEnding = false;
    private float endTimer = 0f;
    public float endDelay = 1f;

    void Update()
    {
        // 終了待機中
        if (isEnding)
        {
            endTimer += Time.deltaTime;

            if (endTimer >= endDelay)
            {
                if (fadeManager != null)
                {
                    fadeManager.StartFade();
                }
            }
            return;
        }

        // 攻撃入力
        if (Input.GetMouseButton(0))
        {
            timer += Time.deltaTime;

            if (timer >= attackInterval)
            {
                AttackAll();
                timer = 0f;

                // ここで攻撃終了状態へ
                isEnding = true;

                // 他の攻撃も止める（重要）
                MouseAttackController.canAttack = false;
            }
        }
        else
        {
            timer = 0f;
        }
    }

    void AttackAll()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag(targetTag);

        foreach (GameObject obj in objs)
        {
            HPmanager hp = obj.GetComponent<HPmanager>();

            if (hp != null)
            {
                hp.TakeDamage(damage, 2f);
            }
        }

        Debug.Log("長押し攻撃！");

        if (SkillManage.Instance != null)
        {
            SkillManage.Instance.ClearSkillData();
        }
    }
}