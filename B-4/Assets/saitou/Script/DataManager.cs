using DG.Tweening;
using System.Collections;
using TMPro;
using Unity.Properties;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI breakPlane;
    [SerializeField] private TextMeshProUGUI skillPoint;
    [SerializeField] private TextMeshProUGUI stardustPoint;

    void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        breakPlane.text = $"”j‰ó‚µ‚½˜f¯‚Ì”\n";
        skillPoint.text = $"Šl“¾zÎ—Ê\n{SPManager.GetSP()}";
        stardustPoint.text = $"¯‚ÌŒ‡•ĞŠl“¾—Ê\n";
    }

}
