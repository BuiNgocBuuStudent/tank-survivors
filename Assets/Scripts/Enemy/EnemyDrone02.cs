using UnityEngine;


public class EnemyDrone02 : EnemyControllerBase
{
    private EnemyDrone02Config _config => (EnemyDrone02Config)EnemyData;

    // TODO (Ngày 2): Implement state machine Chasing / Dashing / Cooldown
    //   - Chase: dùng base.ChaseTarget() bình thường
    //   - Dash: lock direction tại thời điểm bắt đầu, set Rb.velocity = dir * _config.dashSpeed
    //   - Cooldown: Rb.velocity = Vector2.zero, chờ _config.dashCooldown rồi về Chase
    // Tham khảo design doc: dashSpeed=9, dashDuration=0.4s, chaseTime=2s, cooldown=1.5s
}
