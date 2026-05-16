using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class SkillEffect
{
    public enum Type
    {
        Attack,
        Speed,
        Range
    }
    public Type type;
    public float value;
}

//Scriptable Objects(スキルを入れるデータファイル)の作成
[CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable Objects/SkillData")]
public class SkillData : ScriptableObject
{
    public string skillName;
    public string skillDescription;

    public int needPoint;
    public List <SkillData> needSkillData;
    //public SkillData needSkillData;

    public SkillEffect effect;
}
