using UnityEngine;

/// <summary>
/// Config cho EliteArtillery — "Artillery".
/// Kế thừa EnemyConfigBase (có sẵn moveSpeed, damage, intialHealth).
/// </summary>
[CreateAssetMenu(fileName = "EliteArtilleryConfig", menuName = "Scriptable Objects/Enemy/Elite Artillery")]
public class EliteArtilleryConfig : EnemyConfigBase
{
    [Header("--- Position ---")]
    public float stopRange;

    [Header("--- Fire ---")]
    public float aimDuration;
    public float fireCooldown;
    public float shellSpeed;
}
