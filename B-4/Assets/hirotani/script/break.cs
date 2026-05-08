using UnityEngine;

public class OrbitTarget : MonoBehaviour
{
    public Transform player;
    public float speed = 100f;

    void Update()
    {
        if (player != null)
        {
            transform.RotateAround(
                player.position,
                Vector3.forward,
                speed * Time.deltaTime
            );
        }
    }
   /* void SpawnTarget()
    {
        if (player == null) return;

        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float randomDistance = Random.Range(minRadius, maxRadius);

        Vector3 spawnPosition = new Vector3(
            Mathf.Cos(angle) * randomDistance,
            Mathf.Sin(angle) * randomDistance,
            0
        );

        spawnPosition += player.position;

        GameObject obj = Instantiate(
            targetPrefab,
            spawnPosition,
            Quaternion.identity
        );

        // 生成したオブジェクトへ player を渡す
        OrbitTarget orbit = obj.GetComponent<OrbitTarget>();

        if (orbit != null)
        {
            orbit.player = player;
        }
    }
   */
}

