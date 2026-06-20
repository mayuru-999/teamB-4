using UnityEngine;

public class HPmanager : MonoBehaviour
{
   
    [Header("惑星の大きさ設定")]
    [Tooltip("1 = 大きさ1(小), 2 = 大きさ2(中), 3 = 大きさ3(大)")]
    [Range(1, 3)] public int sizeType = 1;

    private float pointMultiplier = 1f;

    public int hp = 100;
    private int currentHP;
    private float currentDustVolume; //  ダストパラメータの値を保持する変数

    [Header("ドロップアイテム")]
    public DropItemData[] dropItems;
    [Header("特殊ドロップ")]
    public GameObject chainDamagePrefab;

    [Range(0f, 1f)]
    public float chainDropRate = 0.1f;
    public bool isSpecial = false;

    //  Spawnerからデータを安全に受け取るための初期化関数 
    public void InitializeStatus(Vector3 healthVector, Vector3 dustVector)
    {
        // sizeType (1〜3) に応じて Vector3 の x, y, z から対応する値を抽出
        switch (sizeType)
        {
            case 1: // 大きさ1 (小)
                hp = Mathf.RoundToInt(healthVector.x);
                currentDustVolume = dustVector.x;
                break;
            case 2: // 大きさ2 (中)
                hp = Mathf.RoundToInt(healthVector.y);
                currentDustVolume = dustVector.y;
                break;
            case 3: // 大きさ3 (大)
                hp = Mathf.RoundToInt(healthVector.z);
                currentDustVolume = dustVector.z;
                break;
            default:
                hp = Mathf.RoundToInt(healthVector.x);
                currentDustVolume = dustVector.x;
                break;
        }

        // ここで currentHP も上書き同期する
        currentHP = hp;
    }

    void Start()
    {
        // 念のため、Spawnerを経由せず配置された場合でも最低限動作するように
        if (currentHP == 0)
        {
            currentHP = hp;
        }

        if (isSpecial)
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();

            if (sr != null)
            {
                sr.color = Color.yellow;
            }
        }
    }

    public void TakeDamage(int damage, float pointMultiplier = 1f)
    {
        currentHP -= damage;

        // 倒れる直前に倍率を保持しておく
        this.pointMultiplier = pointMultiplier;

        Debug.Log(gameObject.name + " HP : " + currentHP);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // 3%で特殊ドロップ
        if (Random.value < 0.03f)
        {
            if (chainDamagePrefab != null)
            {
                Instantiate(
                    chainDamagePrefab,
                    transform.position,
                    Quaternion.identity
                );
            }
        }
        // 97%で通常ドロップ
        else
        {
            foreach (var item in dropItems)
            {
                if (item != null && item.prefab != null)
                {
                    // 💡 必要に応じて、ここで「currentDustVolume」を item.count に掛け算・足し算すると
                    // レベルや大きさによってドロップ数が変化する処理が作れます。
                    for (int i = 0; i < item.count; i++)
                    {
                        Instantiate(
                            item.prefab,
                            transform.position,
                            Quaternion.identity
                        );
                    }
                }
            }
        }

        // 特殊敵の爆発処理（そのまま維持）
        if (isSpecial)
        {
            Collider2D[] hits =
                Physics2D.OverlapCircleAll(
                    transform.position,
                    5f
                );

            foreach (Collider2D hit in hits)
            {
                if (hit.CompareTag("Target"))
                {
                    HPmanager hpComponent =
                        hit.GetComponent<HPmanager>();

                    if (hpComponent != null &&
                         hpComponent != this &&
                          !hpComponent.isSpecial)
                    {
                        hpComponent.TakeDamage(20);
                    }
                }
            }
        }

        Destroy(gameObject);
    }
}