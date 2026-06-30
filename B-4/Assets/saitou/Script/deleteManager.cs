using UnityEngine;

public class deleteManager : MonoBehaviour
{

    private void Start()
    {
        DropItem.destroyedCount = 0;

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