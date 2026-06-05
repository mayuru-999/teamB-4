
using UnityEngine;

public class SpriteSwitcher : MonoBehaviour
{
    public Sprite spriteA;
    public Sprite spriteB;

    private SpriteRenderer sr;
    private float timer = 0f;
    public float switchInterval = 0.5f; // Ø‚è‘Ö‚¦ŠÔŠui•bj

    private bool isA = true;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = spriteA;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= switchInterval)
        {
            timer = 0f;

            // Ø‚è‘Ö‚¦
            isA = !isA;
            sr.sprite = isA ? spriteA : spriteB;
        }
    }
}

