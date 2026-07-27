using UnityEngine;

public class EnemyShooter : EnemyControllerBase
{
    [Header("-----Enemy Shooter 01 Config------")]
    [SerializeField] private EnemyBulletBase _enemyBulletPrefab;

    private float _fireTimer;

    private EnemyShooterConfig _config => (EnemyShooterConfig)EnemyData;

    protected override void OnInit()
    {
        _fireTimer = _config.fireRate;
    }

    protected override void UpdateBehavior()
    {
        _fireTimer -= Time.deltaTime;

        float distance = (Player.transform.position - this.transform.position).magnitude;
        if (distance <= _config.stopRange && _fireTimer <= 0)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        EnemyBulletBase bullet = ObjectPooler.Instance.GetComp(_enemyBulletPrefab);

        Vector2 shootDir = (Player.transform.position - this.transform.position).normalized;
        bullet.Init(_config.bulletSpeed, shootDir);
        bullet.transform.SetPositionAndRotation(this.transform.position, this.transform.rotation);
        bullet.gameObject.SetActive(true);

        _fireTimer = _config.fireRate;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.4f, 0f, 0.3f);
        Gizmos.DrawSphere(transform.position, _config != null ? _config.stopRange : 5f);

        Gizmos.color = new Color(1f, 0.4f, 0f, 1f);
        Gizmos.DrawWireSphere(transform.position, _config != null ? _config.stopRange : 5f);
    }
}
