using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController02 : GunControllerBase
{
    [Header("Shotgun config")]
    [SerializeField] int _numberBullet;
    [SerializeField] float _attackRange;
    void Update()
    {
        Fire();
    }

    protected override void Fire()
    {
        if (_timer >= 0)
            return;

        _timer = _cooldownTime;

        //góc giữa 2 game object
        float centerAngle = Mathf.Atan2(this.transform.up.y, this.transform.up.x) * Mathf.Rad2Deg;

        float anglePerBullet = _attackRange / (_numberBullet - 1);
        float startSpawnAngle = centerAngle + _attackRange / 2;

        for (int i = 0; i < _numberBullet; i++)
        {
            float bulletAngle = (startSpawnAngle - anglePerBullet * i) * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(bulletAngle), Mathf.Sin(bulletAngle));

            BulletBase bullet = ObjectPooler.Instance.GetComp(_bulletPrefab);
            bullet.Init(_bulletSpeed, direction);
            bullet.transform.SetPositionAndRotation(this.transform.position, this.transform.rotation);
            bullet.gameObject.SetActive(true);
        }

    }
}
