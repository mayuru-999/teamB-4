using UnityEngine;

[System.Serializable]
public class skillEffect
{
    public int value;
}

[CreateAssetMenu(fileName = "skillData", menuName = "Scriptable Objects/skillData")]
public class skillData : ScriptableObject
{
    public int id;
    public string skillName;
    public string skillDescription;

    public skillEffect effect;
}
