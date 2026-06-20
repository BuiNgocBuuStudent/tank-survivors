using UnityEngine;

public class EnemyShooter01 : EnemyControllerBase
{
    private EnemyShooterConfig _config => (EnemyShooterConfig)EnemyData;
    // TODO (Ngày 3): Override ChaseTarget() để dừng khi trong stopRange
    // Override UpdateBehavior() để xử lý fire timer và spawn bullet
    // Lưu ý: bullet của enemy cần layer riêng (EnemyBullet)
}
