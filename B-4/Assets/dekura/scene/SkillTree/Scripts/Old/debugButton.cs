using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class debugButton : MonoBehaviour
{
    private SkillManage skillManage;
    private SkillPointManager skillPointManager;

    void Start()
    {
        //マネージャの割り当て
        //skillManage = FindObjectByType<SkillManage>(); <古いらしい
        skillManage = FindAnyObjectByType<SkillManage>();
        skillPointManager = FindAnyObjectByType<SkillPointManager>();
    }
    public void MainScene()
    {
        SceneManager.LoadScene("souma.sence");
    }
    public void ResetClick()
    {
        skillManage.ResetUnlockedSkill();
    }
}
