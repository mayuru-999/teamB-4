using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _text;

    public SkillEffect.Type type;
    private SkillManage skillManage;

    void Awake()
    {
        skillManage = FindAnyObjectByType<SkillManage>();
    }

    // Update is called once per frame
    void Update()
    {
        _text.text = $"{skillManage.getEffect(type)}";
    }
}
