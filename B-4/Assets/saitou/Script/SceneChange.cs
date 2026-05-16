using UnityEngine;

public class SceneChange : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FadeManager.Instance.FadeToScene("souma.sence");
        }
    }
}