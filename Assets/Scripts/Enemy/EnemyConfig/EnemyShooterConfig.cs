using UnityEngine;

[CreateAssetMenu(fileName = "EnemyShooterConfig", menuName = "Scriptable Objects/Enemy/Shooter")]
public class EnemyShooterConfig : EnemyConfigBase
{
    [Header("--- Shoot Behavior ---")]
    public float stopRange;
    public float fireRate;

    [Header("--- Bullet ---")]
    public float bulletSpeed;
}
