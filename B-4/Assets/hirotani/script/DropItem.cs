using UnityEngine;

public class DropItem : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Vector3 targetPosition;

    void Start()
    {
        // ƒJƒƒ‰¶ãÀ•W
        targetPosition =
            Camera.main.ViewportToWorldPoint(
                new Vector3(0f, 1f, 10f));

        targetPosition.z = 0f;
    }

    void Update()
    {
        // ¶ã‚ÖˆÚ“®
        transform.position =
            Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime);

        // “’B‚µ‚½‚çíœ
        if (Vector3.Distance(
            transform.position,
            targetPosition) < 0.1f)
        {
            Destroy(gameObject);
        }
    }
}