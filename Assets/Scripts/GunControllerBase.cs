using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GunControllerBase : MonoBehaviour
{
    [Header("-----Base config-----")]
    [SerializeField] protected Transform _enemy;
    [SerializeField] protected BulletBase _bulletPrefab;
    protected PlayerController _player;
    [SerializeField] private LayerMask _enemyLayerMask;

    [SerializeField] protected float _rotateSpeed, _cooldownTime, _detectTargetRadius;
    protected float _timer;

    [Header("-----Bullet config-----")]
    [SerializeField] protected float _bulletSpeed;
    [SerializeField] protected float _bulletDmg;
    [SerializeField] protected float _bulletLifeTime;

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
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, _detectTargetRadius);
    }
}
