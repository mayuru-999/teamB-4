using UnityEngine;

public class deleteManeger : MonoBehaviour
{

    private void Start()
    {
        Debug.Log("private void Start()");
        SkillManage.Instance.AllClearSkillData();
    }

}