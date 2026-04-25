using UnityEngine;

public class burakku : MonoBehaviour
{
    public Transform target;   // 回転させたいオブジェクト
    public float speed = 100f; // 回転スピード

    void Update()
    {
        if (target != null)
        {
            // 2DではZ軸（forward）を中心に回転
            target.RotateAround(transform.position, Vector3.forward, speed * Time.deltaTime);
        }
    }
}


