using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDrone02Config", menuName = "Scriptable Objects/Enemy/Drone02 Dasher")]
public class EnemyDroneDasherConfig : EnemyConfigBase
{
    [Header("--- Dash ---")]
    public float dashSpeed;
    public float dashDuration;
    public float chaseTimeBeforeDash;
    public float dashCooldown;
}
