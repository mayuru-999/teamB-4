using DG.Tweening;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ToPlaneButtonStatus : MonoBehaviour
{
    [SerializeField] private Image lockSprict;
    [SerializeField] private Image newActionBadge;
    private Button button;
    private bool isUnlocked = false;

    void Start()
    {
        button = gameObject.GetComponent<Button>();

        isUnlocked = SkillManage.Instance.GetFlags("firstCreateUnlocked");
        UpdateUi();
    }

    private void UpdateUi()
    {
        if (isUnlocked == false && SkillPointManager.Instance.starDustPoint >= 20)
        {
            lockSprict.DOFade(0f, 3f)
            .OnComplete(() =>
            {
                SkillManage.Instance.SetFlags("firstCreateUnlocked", true);
                button.enabled = true;
            });
        }
        else if (isUnlocked == true)
        {
            lockSprict.color = Color.clear;
            button.enabled = true;
        }
    }
}
