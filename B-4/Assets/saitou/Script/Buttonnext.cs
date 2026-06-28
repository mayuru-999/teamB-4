using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scene2 : MonoBehaviour
{
    public void change_button()
    {
        // スキル管理のフラグを設定
        if (SkillManage.Instance != null)
        {
            SkillManage.Instance.SetReturnFromSkillTreeFlag();
        }
        // シーン遷移
        SceneManager.LoadScene("souma.sence");
    }
}