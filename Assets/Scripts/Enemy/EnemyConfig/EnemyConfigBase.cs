using UnityEngine;


[CreateAssetMenu(fileName = "EnemyConfig", menuName = "Scriptable Objects/Enemy/Base")]
public class EnemyConfigBase : ScriptableObject
{
    public float moveSpeed;
    public float damage;
    public float intialHealth;
}
