using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    void Update()
    {
        // 左クリックを検知
        if (Input.GetMouseButtonDown(0))
        {
            // シーン切り替え
            SceneManager.LoadScene("souma.sence");
        }
    }
}