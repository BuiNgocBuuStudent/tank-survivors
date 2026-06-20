using UnityEngine;
using System.Collections;


public class EnemyElite03 : EnemyControllerBase
{
    private EnemyElite03Config _config => (EnemyElite03Config)EnemyData;
    // TODO (Ngày 5):
    // Tạo WarningCircle prefab trong Unity (SpriteRenderer tròn, đỏ, alpha 40%, không Collider)
    // Override ChaseTarget() để dừng khi trong artilleryStopRange
    // Implement FireSequence() Coroutine: lock pos → show warning → wait → TriggerExplosion
    // Animate WarningCircle scale 0 → 1 trong aimDuration (tùy chọn)
    // Lưu ý quan trọng: check gameObject.activeInHierarchy trong Coroutine trước khi trigger explosion
}
