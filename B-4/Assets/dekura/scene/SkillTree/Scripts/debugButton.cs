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
    //クリック時の処理
    public void OnClick()
    {
        Debug.Log($"Attackのvalueは{skillManage.getEffect(SkillEffect.Type.Attack)}");
        Debug.Log($"Speedのvalueは{skillManage.getEffect(SkillEffect.Type.Speed)}");
        Debug.Log($"Rangeのvalueは{skillManage.getEffect(SkillEffect.Type.Range)}");
        Debug.Log($"所持中のコストは{skillPointManager.skillPoint}");
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
