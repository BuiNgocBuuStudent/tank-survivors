using UnityEngine;

public class EnemyDrone03 : EnemyControllerBase
{
    [Header("-----Enemy Boomber Config------")]
    [SerializeField] private EnemyBullet _enemyBulletPrefab;
    private EnemyDrone03Config _config => (EnemyDrone03Config)EnemyData;

    protected override void OnDie(float lastDmg)
    {
        base.OnDie(lastDmg);
        SpawnDeathBullets();
    }
    private void SpawnDeathBullets()
    {
        int bulletCount = _config.deathBulletCount;
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = (360f / bulletCount) * i;
            float rad = angle * Mathf.Deg2Rad;
            Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

            EnemyBullet bullet = ObjectPooler.Instance.GetComp(_enemyBulletPrefab);

            // Xoay sprite theo hướng di chuyển (trừ 90° vì sprite mặc định trỏ lên)
            Quaternion rotation = Quaternion.Euler(0f, 0f, angle - 90);

            bullet.Init(_config.deathBulletSpeed, dir);
            bullet.transform.SetPositionAndRotation(this.transform.position, rotation);
            bullet.gameObject.SetActive(true);
        }
    }
}
