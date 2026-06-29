using UnityEngine;

public class HPmanager : MonoBehaviour
{
    [Header("惑星の大きさ設定")]
    [Tooltip("1 = 大きさ1(小), 2 = 大きさ2(中), 3 = 大きさ3(大)")]
    [Range(1, 3)] public int sizeType = 1;

    private float pointMultiplier = 1f;

    public int hp = 100;
    private int currentHP;
    private float currentDustVolume; // ダストパラメータの値を保持する変数

    [Header("ドロップアイテム")]
    public DropItemData[] dropItems;
    [Header("特殊ドロップ")]
    public GameObject chainDamagePrefab;
    private bool isDead = false;

    // Spawnerからデータを安全に受け取るための初期化関数 
    public void InitializeStatus(Vector3 healthVector, Vector3 dustVector)
    {
        switch (sizeType)
        {
            case 1:
                hp = Mathf.RoundToInt(healthVector.x);
                currentDustVolume = dustVector.x;
                break;
            case 2:
                hp = Mathf.RoundToInt(healthVector.y);
                currentDustVolume = dustVector.y;
                break;
            case 3:
                hp = Mathf.RoundToInt(healthVector.z);
                currentDustVolume = dustVector.z;
                break;
            default:
                hp = Mathf.RoundToInt(healthVector.x);
                currentDustVolume = dustVector.x;
                break;
        }

        currentHP = hp;
    }

    void Start()
    {
        if (currentHP == 0)
        {
            currentHP = hp;
        }
    }

    public void TakeDamage(int damage, float pointMultiplier = 1f)
    {
        if (isDead) return;

        currentHP -= damage;

        // ここで渡された倍率を保存（ビッグバン時は 2f が入る）
        this.pointMultiplier = pointMultiplier;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        ChainDamage chain = GetComponent<ChainDamage>();

        if (chain != null)
        {
            chain.Explode();
            return;
        }

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
                Destroy(gameObject);
            }
        }
        else
        {
            int totalDropCount = 0;

            foreach (var item in dropItems)
            {
                if (item != null && item.prefab != null)
                {
                    // 変更点：本来の個数に倍率を掛けて、生成する総数を計算
                    // Mathf.RoundToInt で四捨五入して整数にします（1.5倍などにも対応可能）
                    int finalCount = Mathf.RoundToInt(item.count * pointMultiplier);

                    totalDropCount += finalCount;

                    // 変更点：計算した finalCount 分だけループして生成
                    for (int i = 0; i < finalCount; i++)
                    {
                        Instantiate(
                            item.prefab,
                            transform.position,
                            Quaternion.identity
                        );
                    }
                }
            }

            // ここでDP加算

            Destroy(gameObject);
        }
    }
}