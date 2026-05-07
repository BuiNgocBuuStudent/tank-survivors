using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StateUpgrade", menuName = "Scriptable Objects/State Upgrade")]
public class StatUpgradeConfig : ScriptableObject
{
    public string statName;
    public int maxLevel = 10;
    public float[] values;
    public int[] costs;
}
