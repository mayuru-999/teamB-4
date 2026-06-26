using UnityEngine;

public class DeleteByTag : MonoBehaviour
{
    public GameObject resultPanel;

    private Renderer myRenderer;
    private Color originalColor;

    public float blinkSpeed = 5f;
    private bool isBlinking = false;

    public string targetTag = "Target";
    public int damage = 10;
    public float attackInterval = 1f;

    private float timer = 0f;

    public float endDelay = 1f;

    private bool isEnding = false;
    private float endTimer = 0f;

    private bool canUseBigBang = false;

    void Start()
    {
        if (resultPanel != null)
            resultPanel.SetActive(false);

        myRenderer = GetComponent<Renderer>();

        if (myRenderer != null)
        {
            myRenderer.material = new Material(myRenderer.material);
            originalColor = myRenderer.material.color;
        }

        if (SkillManage.Instance != null)
        {
            SkillManage.Instance.RefreshReferences();
            SkillManage.Instance.CheckAndIncrementVisitCount();
        }
    }

    void Update()
    {
        HandleBlink();

        if (isEnding)
        {
            endTimer += Time.deltaTime;

            if (endTimer >= endDelay)
            {
                if (resultPanel != null)
                    resultPanel.SetActive(true);
            }

            return;
        }

        CheckBigBangReady();
        HandleInput();
    }

    void HandleBlink()
    {
        if (isBlinking && myRenderer != null)
        {
            float t = Mathf.Sin(Time.time * blinkSpeed) * 0.5f + 0.5f;
            Color blinkColor = Color.Lerp(originalColor, Color.red, t);
            myRenderer.material.color = blinkColor;
        }
    }

    void StopBlink()
    {
        isBlinking = false;

        if (myRenderer != null)
            myRenderer.material.color = originalColor;
    }

    void CheckBigBangReady()
    {
        if (SkillManage.Instance == null) return;

        if (SkillManage.Instance.MainVisitCount >= 1)
        {
            if (!canUseBigBang)
            {
                canUseBigBang = true;
                isBlinking = true;

                Debug.Log("ビッグバン準備OK");
            }
        }
    }

    void HandleInput()
    {
        if (Input.GetMouseButton(0))
        {
            if (canUseBigBang)
            {
                timer += Time.deltaTime;

                if (timer >= attackInterval)
                    TriggerBigBang();
            }
        }
        else
        {
            timer = 0f;
        }
    }

    void TriggerBigBang()
    {
        AttackAll();
        timer = 0f;

        if (SkillManage.Instance != null)
        {
            SkillManage.Instance.LvUpdate();
            SkillManage.Instance.ResetVisitCount();
        }

        StopBlink();

        isEnding = true;
        canUseBigBang = false;
        MouseAttackController.canAttack = false;

        Debug.Log("ビッグバン発動");
    }

    void AttackAll()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag(targetTag);

        foreach (GameObject obj in objs)
        {
            HPmanager hp = obj.GetComponent<HPmanager>();

            if (hp != null)
                hp.TakeDamage(damage, 2f);
        }

        if (SkillManage.Instance != null)
            SkillManage.Instance.ClearSkillData();

        Debug.Log("全体攻撃完了");
    }
}