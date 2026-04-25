using UnityEngine;

public class playerMove : MonoBehaviour
{
    Vector3 mousePos, pos;

    void Update()
    {
        mousePos=Input.mousePosition;
        mousePos.z = 10f;
        pos=Camera.main.ScreenToWorldPoint(mousePos);
        transform.position = pos;
        Debug.Log($"{pos}");
    }
}