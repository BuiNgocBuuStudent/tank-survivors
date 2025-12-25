using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDataBase", menuName = "Scriptable Objects/Enemy")]
public class EnemyDataBase : ScriptableObject
{
    public float moveSpeed;
    public float damage;
    public float intialHealth;
}
