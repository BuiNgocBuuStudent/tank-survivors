using UnityEngine;

/// <summary>
/// Config cho Drone03 — "Bomber".
/// Kế thừa EnemyConfigBase (có sẵn moveSpeed, damage, intialHealth).
/// </summary>
[CreateAssetMenu(fileName = "EnemyDrone03Config", menuName = "Scriptable Objects/Enemy/Drone03 Bomber")]
public class EnemyDrone03Config : EnemyConfigBase
{
    [Header("--- Death Burst ---")]
    [Tooltip("Số đạn tỏa ra khi chết (gợi ý: 12)")] public int deathBulletCount;
    [Tooltip("Tốc độ đạn (gợi ý: 4.5)")] public float deathBulletSpeed;
    [Tooltip("Lifetime đạn (gợi ý: 0.6s)")] public float deathBulletLifetime;
    [Tooltip("Damage mỗi viên (gợi ý: 8)")] public float deathBulletDamage;
}
