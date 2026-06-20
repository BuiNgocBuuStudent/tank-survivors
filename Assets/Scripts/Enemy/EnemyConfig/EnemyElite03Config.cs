using UnityEngine;

/// <summary>
/// Config cho EnemyElite03 — "Artillery".
/// Kế thừa EnemyConfigBase (có sẵn moveSpeed, damage, intialHealth).
/// </summary>
[CreateAssetMenu(fileName = "EnemyElite03Config", menuName = "Scriptable Objects/Enemy/Elite03 Artillery")]
public class EnemyElite03Config : EnemyConfigBase
{
    [Header("--- Position ---")]
    [Tooltip("Khoảng cách dừng và bắn (gợi ý: 7.0)")] public float stopRange;

    [Header("--- Fire ---")]
    [Tooltip("Thời gian telegraph — WarningCircle hiển thị (gợi ý: 1.5s)")] public float aimDuration;
    [Tooltip("Cooldown giữa 2 lần bắn (gợi ý: 4.0s)")] public float fireCooldown;

    [Header("--- Explosion ---")]
    [Tooltip("Bán kính vụ nổ (gợi ý: 2.5)")] public float explosionRadius;
    [Tooltip("Damage vụ nổ (gợi ý: 45)")] public float explosionDamage;
}
