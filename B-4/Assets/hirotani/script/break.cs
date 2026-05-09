using UnityEngine;

public class OrbitTarget : MonoBehaviour
{
    [Header("‰ñ“]İ’è")]
    public Transform player;
    public float speed = 100f;
   

    /*[Header("‘Ì—Í")]
    public int maxHP = 3;

    private int currentHP;
    private float angle;
    
    void Start()
    {
        // ‰Šú‘Ì—Í
        currentHP = maxHP;
    }
    */
    void Update() {
        if (player != null)
        { transform.RotateAround(player.position, Vector3.forward, speed * Time.deltaTime); }
    }
    /*
    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        Debug.Log(gameObject.name + " HP : " + currentHP);

        if (currentHP <= 0)
        {
            Destroy(gameObject);
        }
    }
    */
}

