using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GunControllerBase : MonoBehaviour
{
    [Header("-----Base config-----")]
    [SerializeField] protected BulletBase _bulletPrefab;
    protected PlayerControllerBase _player;
    [SerializeField] protected LayerMask _enemyLayerMask;
    protected Transform _enemy;
    [SerializeField] protected GunState _gunState;

    [SerializeField] protected float _cooldownTime, _rotateSpeed, _detectTargetRadius;
    protected float _timer;
    protected Vector2 _movement;

    [Header("-----Bullet config-----")]
    [SerializeField] protected float _bulletSpeed;

    protected enum GunState
    {
        ROTATE,
        SHOOT
    }
    public void Init()
    {
        _player = GameManager.Instance.Player;
        _timer = 0;
    }
    private void FixedUpdate()
    {
        if (_gunState != GunState.SHOOT)
            this.transform.Rotate(new Vector3(0, 0, _rotateSpeed * Time.deltaTime));
        _timer -= Time.deltaTime;

    }
    protected abstract void Fire();

    protected virtual bool DetectTarget(float radius)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, _enemyLayerMask);

        if (hits.Length == 0)
        {
            _enemy = null;
            _gunState = GunState.ROTATE;
            return false;
        }

        // Ưu tiên mục tiêu gần nhất
        Collider2D closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D hit in hits)
        {
            if (hit == null) continue;
            float distance = Vector2.Distance(transform.position, hit.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = hit;
            }
        }

        _enemy = closestTarget.transform;
        return true;
    }

    protected bool AimTarget()
    {
        if (_enemy == null)
            return false;

        _movement = _enemy.transform.position - this.transform.position;

        float angle = Mathf.Atan2(_movement.y, _movement.x) * Mathf.Rad2Deg - 90;
        Quaternion quaternion = this.transform.rotation;
        quaternion.eulerAngles = new Vector3(0, 0, angle);
        this.transform.rotation = quaternion;

        return true;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, _detectTargetRadius);
    }
}
