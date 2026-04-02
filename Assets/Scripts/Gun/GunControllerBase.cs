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

    [SerializeField] protected float _cooldownTime;
    protected float _timer;

    [Header("-----Bullet config-----")]
    [SerializeField] protected float _bulletSpeed;

    public void Init()
    {
        _player = GameManager.Instance.Player;
        _timer = 0;
    }
    private void FixedUpdate()
    {
        _timer -= Time.deltaTime;

    }
    protected abstract void Fire();
}
