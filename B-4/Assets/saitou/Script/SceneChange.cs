using UnityEngine;

public class SceneChange : MonoBehaviour
{

    static bool isClick = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SaveManager.Instance.DeleteSaveData();
        }

        if (Input.GetMouseButtonDown(0) && !isClick)
        {
            isClick = true;
            SaveManager.Instance.Load();
            FadeManager.Instance.FadeToScene("souma.sence");
        }
    }
}