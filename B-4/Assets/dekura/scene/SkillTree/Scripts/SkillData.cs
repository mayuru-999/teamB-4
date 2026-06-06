using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class SkillEffect
{
    public enum Type
    {
        Attack,             //攻撃力
        Speed,              //攻撃速度
        Range,              //今撃範囲
        ColorChainAttack,   //スキル攻撃力
        SlipAriaAttack,     //スキル攻撃力
        PlaneVolume,        //惑星量増加
        PlaneLv             //惑星サイズ増加
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
    [HideInInspector] public int needPoint;

    public SkillEffect effect;
}
