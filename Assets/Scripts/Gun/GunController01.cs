using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController01 : GunControllerBase
{
    [SerializeField] bool _isDoubleBullet;
    [SerializeField] float _angle;
    void Update()
    {
        Fire();
    }
    protected override void Fire()
    {
        if (_timer >= 0)
            return;

        _timer = _cooldownTime;

        //if (_isDoubleBullet)
            this.SpawnBullet(this.transform.up);
    }
    private void SpawnBullet(Vector2 direction)
    {
        BulletBase bullet = ObjectPooler.Instance.GetComp(_bulletPrefab);
        bullet.Init(_bulletSpeed, direction);
        bullet.transform.SetPositionAndRotation(this.transform.position, this.transform.rotation);
        bullet.gameObject.SetActive(true);
    }
}
