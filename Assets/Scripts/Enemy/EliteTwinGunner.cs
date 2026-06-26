using System.Collections;
using UnityEngine;

public class EliteTwinGunner : EnemyControllerBase
{
    [Header("--- Elite Twin Gunner Config ---")]
    [SerializeField] private EnemyBulletBase _bulletPrefab;

    private EliteTwinGunnerConfig _config => (EliteTwinGunnerConfig)EnemyData;

    private bool _isFiring;
    protected override void OnInit()
    {
        _isFiring = false;
    }

    protected override void ChaseTarget()
    {
        if (_isFiring) return;

        float distance = (Player.transform.position - this.transform.position).magnitude;
        if (distance <= _config.stopRange)
        {
            Rb.velocity = Vector2.zero;
            FacePlayer();
        }
        else
        {
            base.ChaseTarget();
        }
    }

    protected override void UpdateBehavior()
    {
        if (_isFiring) return;

        float distance = (Player.transform.position - this.transform.position).magnitude;
        if (distance <= _config.stopRange)
        {
            StartCoroutine(BurstSequence());
        }
    }

    private IEnumerator BurstSequence()
    {
        _isFiring = true;

        yield return new WaitForSeconds(0.3f);
        if (!gameObject.activeInHierarchy) yield break;

        for (int i = 0; i < _config.burstCount; i++)
        {
            if (!gameObject.activeInHierarchy) yield break;

            ShootTwinLane();

            yield return new WaitForSeconds(_config.fireRate);
        }

        if (!gameObject.activeInHierarchy) yield break;

        // Cooldown sau burst
        yield return new WaitForSeconds(_config.burstCooldown);

        _isFiring = false;
    }


    private void ShootTwinLane()
    {
        Vector2 dir = (Player.transform.position - this.transform.position).normalized;

        // Hướng vuông góc với hướng bắn
        Vector2 perpendicular = new Vector2(-dir.y, dir.x);

        Vector2 laneLeft  = (Vector2)this.transform.position + perpendicular *  _config.laneOffset;
        Vector2 laneRight = (Vector2)this.transform.position + perpendicular * -_config.laneOffset;

        SpawnBullet(laneLeft,  dir);
        SpawnBullet(laneRight, dir);
    }

    private void SpawnBullet(Vector2 spawnPos, Vector2 dir)
    {
        EnemyBulletBase bullet = ObjectPooler.Instance.GetComp(_bulletPrefab);
        if (bullet == null) return;

        bullet.Init(_config.bulletSpeed, dir);
        bullet.transform.SetPositionAndRotation(spawnPos, this.transform.rotation);
        bullet.gameObject.SetActive(true);
    }

    private void FacePlayer()
    {
        Vector2 dir = Player.transform.position - this.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90f;
        Quaternion rot = this.transform.rotation;
        rot.eulerAngles = new Vector3(0f, 0f, angle);
        this.transform.rotation = rot;
    }

    protected override void OnDie(float lastDmg)
    {
        _isFiring = false;
        StopAllCoroutines();
        base.OnDie(lastDmg);
    }


    private void OnDrawGizmosSelected()
    {
        if (_config == null) return;

        // Vùng dừng và bắn — màu tím (elite color)
        Gizmos.color = new Color(0.6f, 0f, 1f, 0.2f);
        Gizmos.DrawSphere(transform.position, _config.stopRange);

        Gizmos.color = new Color(0.6f, 0f, 1f, 0.9f);
        Gizmos.DrawWireSphere(transform.position, _config.stopRange);

        // Vẽ 2 làn đạn khi đang trong Scene view để debug laneOffset
        if (_config.laneOffset > 0f && Application.isPlaying && _isFiring)
        {
            Vector2 dir = Vector2.up; // placeholder khi không có player
            Vector2 perp = new Vector2(-dir.y, dir.x);
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position + (Vector3)(perp *  _config.laneOffset), dir * 3f);
            Gizmos.DrawRay(transform.position + (Vector3)(perp * -_config.laneOffset), dir * 3f);
        }
    }
}
