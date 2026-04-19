using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "Scriptable Objects/Enemy")]
public class EnemyConfig : ScriptableObject
{
    public float moveSpeed;
    public float damage;
    public float intialHealth;
}
