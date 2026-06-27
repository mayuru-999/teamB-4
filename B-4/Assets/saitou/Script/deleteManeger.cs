using UnityEngine;

public class deleteManeger : MonoBehaviour
{

    private void Start()
    {
        Debug.Log("private void Start()");

        if (SkillManage.Instance != null)
        {
            Debug.Log("SkillManage.Instance.AllClearSkillData()");
            SkillManage.Instance.AllClearSkillData();
        }

        
    }

}