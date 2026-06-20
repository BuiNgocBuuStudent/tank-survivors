using UnityEngine;


[CreateAssetMenu(fileName = "EnemyDrone02Config", menuName = "Scriptable Objects/Enemy/Drone02 Dasher")]
public class EnemyDrone02Config : EnemyConfigBase
{
    [Header("--- Dash ---")]
    [Tooltip("Tốc độ lao (gợi ý: 9.0)")] public float dashSpeed;
    [Tooltip("Thời gian giữ dash (gợi ý: 0.4s)")] public float dashDuration;
    [Tooltip("Thời gian chase trước khi dash (gợi ý: 2.0s)")] public float chaseTimeBeforeDash;
    [Tooltip("Cooldown sau dash (gợi ý: 1.5s)")] public float dashCooldown;
}
