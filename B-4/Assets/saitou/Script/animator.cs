using UnityEngine;

public class animator : MonoBehaviour
{
    [SerializeField] public float Speed = 1;
    void Start()
    {
        GetComponent<Animator>().speed = Speed;
    }
}
