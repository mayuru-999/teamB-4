using UnityEngine;

public class deleteManeger : MonoBehaviour
{

    private void Start()
    {
       
        if (SkillManage.Instance != null)
        {
            Debug.Log("SkillManage.Instance != null");
            SkillManage.Instance.AllClearSkillData();
            Debug.Log("SkillManage.Instance.AllClearSkillData()");
        }
        
        PlayerPrefs.DeleteAll();
        Debug.Log("PlayerPrefs.DeleteAll()");

    }

}