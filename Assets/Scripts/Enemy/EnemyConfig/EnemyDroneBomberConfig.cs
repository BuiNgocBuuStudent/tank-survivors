using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDrone03Config", menuName = "Scriptable Objects/Enemy/Drone03 Bomber")]
public class EnemyDroneBomberConfig : EnemyConfigBase
{
    [Header("--- Death Burst ---")]
    public int deathBulletCount;
    public float deathBulletSpeed;
}
