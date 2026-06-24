using UnityEngine;

/// <summary>
/// Config cho EliteArtillery — "Artillery".
/// Kế thừa EnemyConfigBase (có sẵn moveSpeed, damage, intialHealth).
/// </summary>
[CreateAssetMenu(fileName = "EliteArtilleryConfig", menuName = "Scriptable Objects/Enemy/Elite Artillery")]
public class EliteArtilleryConfig : EnemyConfigBase
{
    [Header("--- Position ---")]
    [Tooltip("Khoảng cách dừng và bắn (gợi ý: 7.0)")] public float stopRange;

    [Header("--- Fire ---")]
    [Tooltip("Thời gian telegraph — WarningCircle hiển thị (gợi ý: 1.5s)")] public float aimDuration;
    [Tooltip("Cooldown giữa 2 lần bắn (gợi ý: 4.0s)")] public float fireCooldown;
    [Tooltip("Tốc độ đạn pháo bay (gợi ý: 6.0)")] public float shellSpeed;
}
