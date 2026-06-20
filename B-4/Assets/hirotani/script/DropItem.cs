using UnityEngine;

public class DropItem : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float spreadDistance = 2f;
    public float spreadTime = 0.5f;

    public float pointMultiplier = 1f; // ©’Ç‰Á

    private Vector3 targetPosition;
    private Vector3 startPosition;
    private Vector3 spreadTarget;

    private float timer = 0f;
    private bool isSpreading = true;

    public static int destroyedCount = 0;

    void Start()
    {
        targetPosition = Camera.main.ViewportToWorldPoint(
            new Vector3(0f, 1f, 10f));
        targetPosition.z = 0f;

        startPosition = transform.position;

        Vector2 randomDir = Random.insideUnitCircle.normalized;
        spreadTarget = startPosition + (Vector3)(randomDir * spreadDistance);
    }

    void Update()
    {
        transform.Rotate(0, 0, 180f * Time.deltaTime);

        if (isSpreading)
        {
            timer += Time.deltaTime;
            float t = timer / spreadTime;
            t = 1f - Mathf.Pow(1f - t, 3f);

            transform.position = Vector3.Lerp(startPosition, spreadTarget, t);

            if (t >= 1f)
            {
                isSpreading = false;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                destroyedCount += Mathf.RoundToInt(1 * pointMultiplier);

                SPManager.AddSP(Mathf.RoundToInt(pointMultiplier));

                Debug.Log("Á‚¦‚½”: " + destroyedCount);

                Destroy(gameObject);
            }
        }
    }
}