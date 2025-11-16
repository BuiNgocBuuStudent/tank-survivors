using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GunControllerBase : MonoBehaviour
{
    [Header("-----Base config-----")]
    [SerializeField] protected BulletBase _bulletPrefab;
    protected PlayerControllerBase _player;
    [SerializeField] private LayerMask _enemyLayerMask;
    protected Transform _enemy;

    [SerializeField] protected float _cooldownTime, _rotateSpeed, _detectTargetRadius;
    protected float _timer;
    protected Vector2 _movement;

    [Header("-----Bullet config-----")]
    [SerializeField] protected float _bulletSpeed;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void Init()
    {
        _player = GameManager.Instance.Player;

        _timer = 0;
    }
    // Update is called once per frame
    void Update()
    {

    }

    protected abstract void Fire();

    protected virtual bool DetectTarget(float radius)
    {

        Collider2D[] targetList = Physics2D.OverlapCircleAll(transform.position, radius, _enemyLayerMask);

        foreach (Collider2D target in targetList)
        {
            if (target != null)
            {
                _enemy = target.transform;
                return true;
            }
            else
                _enemy = null;
        }

        return false;
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
