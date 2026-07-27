using UnityEngine;

[CreateAssetMenu(fileName = "EnemyElite02Config", menuName = "Scriptable Objects/Enemy/Elite Twin Gunner")]
public class EliteTwinGunnerConfig : EnemyConfigBase
{
    [Header("--- Position ---")]
    public float stopRange;

    [Header("--- Burst ---")]
    public float fireRate;
    public int burstCount;
    public float burstCooldown;

    [Header("--- Twin Lane Bullet ---")]
    public float bulletSpeed;
    public float laneOffset;
}
