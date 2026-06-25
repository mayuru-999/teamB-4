using UnityEngine;

public class DeleteByTag : MonoBehaviour
{
    public WhiteFadeManager fadeManager;
    public string targetTag = "Target";
    public int damage = 10;
    public float attackInterval = 1f;

    private float timer = 0f;

    // 終了処理用
    private bool isEnding = false;
    private float endTimer = 0f;
    public float endDelay = 1f;

    void Start()
    {
        // シーンが新しくなったので、SkillManage内のUI等の参照をリフレッシュする
        if (SkillManage.Instance != null)
        {
            SkillManage.Instance.RefreshReferences();

            // カウントのチェックを実行
            SkillManage.Instance.CheckAndIncrementVisitCount();
            Debug.Log($"シーンカウント: {SkillManage.Instance.MainVisitCount}回目");
        }
    }

    void Update()
    {
        // 終了待機モード
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

        // 攻撃の処理（マウス長押し）
        if (Input.GetMouseButton(0))
        {
            // ★条件追加: カウントが16の時だけチャージ（タイマー）が進む
            if (SkillManage.Instance != null && SkillManage.Instance.MainVisitCount == 16)
            {
                timer += Time.deltaTime;

                if (timer >= attackInterval)
                {
                    TriggerBigBang();
                }
            }
        }
        else
        {
            timer = 0f;
        }
    }

    /// <summary>
    /// ビッグバン（全体攻撃、カウントリセット、終了モード移行）を実行する
    /// </summary>
    private void TriggerBigBang()
    {
        AttackAll();
        timer = 0f;

        // ★追加: 使用後にカウントをリセット（0にする、または1にする場合は適宜変更してください）
        if (SkillManage.Instance != null)
        {
            // メソッドを呼び出して安全にリセットする
            SkillManage.Instance.ResetVisitCount();
        }

        // 攻撃終了モードに移行
        isEnding = true;

        // 通常の攻撃を止める
        MouseAttackController.canAttack = false;
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

        Debug.Log("全体への攻撃（ビッグバン）完了");

        if (SkillManage.Instance != null)
        {
            SkillManage.Instance.ClearSkillData();
        }
    }
}