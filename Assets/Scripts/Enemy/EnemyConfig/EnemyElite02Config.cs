using UnityEngine;

/// <summary>
/// Config cho EnemyElite02 — "Twin Gunner".
/// Kế thừa EnemyConfigBase (có sẵn moveSpeed, damage, intialHealth).
/// </summary>
[CreateAssetMenu(fileName = "EnemyElite02Config", menuName = "Scriptable Objects/Enemy/Elite02 Twin Gunner")]
public class EnemyElite02Config : EnemyConfigBase
{
    [Header("--- Position ---")]
    [Tooltip("Khoảng cách dừng và bắn (gợi ý: 6.0)")] public float stopRange;

    [Header("--- Burst ---")]
    [Tooltip("Giây giữa mỗi phát trong burst (gợi ý: 0.4s)")] public float fireRate;
    [Tooltip("Số lần bắn mỗi burst (gợi ý: 6)")] public int burstCount;
    [Tooltip("Cooldown sau burst (gợi ý: 2.0s)")] public float burstCooldown;

    [Header("--- Twin Lane Bullet ---")]
    [Tooltip("Tốc độ đạn (gợi ý: 5.5)")] public float bulletSpeed;
    [Tooltip("Damage mỗi viên (gợi ý: 10)")] public float bulletDamage;
    [Tooltip("Lifetime đạn (gợi ý: 2.5s)")] public float bulletLifetime;
    [Tooltip("Khoảng cách 2 làn đạn — đủ rộng để né được (gợi ý: 0.6)")] public float laneOffset;
}
