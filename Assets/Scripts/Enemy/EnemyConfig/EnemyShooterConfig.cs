using UnityEngine;

/// <summary>
/// Config cho EnemyShooter01 — "Shooter".
/// Kế thừa EnemyConfigBase (có sẵn moveSpeed, damage, intialHealth).
/// </summary>
[CreateAssetMenu(fileName = "EnemyShooterConfig", menuName = "Scriptable Objects/Enemy/Shooter01")]
public class EnemyShooterConfig : EnemyConfigBase
{
    [Header("--- Shoot Behavior ---")]
    [Tooltip("Khoảng cách dừng lại và bắn (gợi ý: 5.0)")] public float stopRange;
    [Tooltip("Giây giữa mỗi lần bắn (gợi ý: 1.2s)")] public float fireRate;

    [Header("--- Bullet ---")]
    [Tooltip("Tốc độ đạn (gợi ý: 6.0)")] public float bulletSpeed;
}
