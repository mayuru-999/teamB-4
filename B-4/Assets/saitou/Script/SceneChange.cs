using UnityEngine;

public class SceneChange : MonoBehaviour
{

    static bool isClick = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isClick)
        {
            isClick = true;
            FadeManager.Instance.FadeToScene("souma.sence");
        }
    }
}