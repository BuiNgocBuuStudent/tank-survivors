using UnityEngine;

public class EnemyDrone03 : EnemyControllerBase
{
    private EnemyDrone03Config _config => (EnemyDrone03Config)EnemyData;
    // TODO (Ngày 4): Override OnDie() để spawn vòng đạn trước khi gọi base.OnDie()
    // Tham khảo pattern: SpawnClusterFragments() trong Bullet03.cs
}
