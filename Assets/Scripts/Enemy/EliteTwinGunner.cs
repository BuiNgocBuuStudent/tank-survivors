using UnityEngine;

public class EliteTwinGunner : EnemyControllerBase
{
    private EliteTwinGunnerConfig _config => (EliteTwinGunnerConfig)EnemyData;
    // TODO (Ngày 6): Override ChaseTarget() để dừng khi trong eliteStopRange
    // Implement burst Coroutine với ShootTwinLane()
    // Kỹ thuật twin lane: perpendicular = new Vector2(-dir.y, dir.x)
}
