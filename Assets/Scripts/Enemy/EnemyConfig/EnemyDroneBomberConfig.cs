using UnityEngine;

/// <summary>
/// Config cho Drone03 — "Bomber".
/// Kế thừa EnemyConfigBase (có sẵn moveSpeed, damage, intialHealth).
/// </summary>
[CreateAssetMenu(fileName = "EnemyDrone03Config", menuName = "Scriptable Objects/Enemy/Drone03 Bomber")]
public class EnemyDroneBomberConfig : EnemyConfigBase
{
    [Header("--- Death Burst ---")]
    [Tooltip("Số đạn tỏa ra khi chết (gợi ý: 12)")] public int deathBulletCount;
    [Tooltip("Tốc độ đạn (gợi ý: 4.5)")] public float deathBulletSpeed;
}
