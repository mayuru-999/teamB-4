using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeByKey : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("SkillTree_debug");
        }
    }
}