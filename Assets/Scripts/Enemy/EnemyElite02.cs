using UnityEngine;

public class EnemyElite02 : EnemyControllerBase
{
    private EnemyElite02Config _config => (EnemyElite02Config)EnemyData;
    // TODO (Ngày 6): Override ChaseTarget() để dừng khi trong eliteStopRange
    // Implement burst Coroutine với ShootTwinLane()
    // Kỹ thuật twin lane: perpendicular = new Vector2(-dir.y, dir.x)
}
