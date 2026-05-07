using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillUpgrade", menuName = "Scriptable Objects/Skill Upgrade")]
public class SkillUpgradeConfig : ScriptableObject
{
    public string skillName;
    public string description;
    public int tier;
    public int cost;
    public string tankId;
}
