using UnityEngine;

[System.Serializable]
public class SkillEffect
{
    public int value;
}

[CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable Objects/SkillData")]
public class SkillData : ScriptableObject
{
    public int id;
    public string skillName;
    public string skillDescription;

    public SkillEffect effect;
}
