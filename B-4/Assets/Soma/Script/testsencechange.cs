using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeByKey : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "SkillTree";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}