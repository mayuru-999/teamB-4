using UnityEngine;

public class reset : MonoBehaviour
{
    private SkillManage skillManage;
    void Start()
    {
        skillManage = FindAnyObjectByType<SkillManage>();
    }

    public void OnClick()
    {
        skillManage.ResetUnlockedSkill();
    }
}
