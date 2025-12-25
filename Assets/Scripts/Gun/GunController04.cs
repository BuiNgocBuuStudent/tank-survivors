using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController04 : GunControllerBase
{
    void Update()
    {
        if (!DetectTarget(_detectTargetRadius))
            return;

        Vector2 dir = _enemy.position - this.transform.position;

        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, dir, dir.magnitude);
        Debug.DrawRay(this.transform.position, dir, Color.red);

        Fire();
    }
    protected override void Fire()
    {
        if (_timer >= 0 || !AimTarget())
            return;

        _timer = _cooldownTime;
        _gunState = GunState.SHOOT;

        BulletBase bullet = ObjectPooler.Instance.GetComp(_bulletPrefab);
        bullet.Init(_bulletSpeed, _movement);
        bullet.transform.SetPositionAndRotation(this.transform.position, this.transform.rotation);
        bullet.gameObject.SetActive(true);

        _gunState = GunState.ROTATE;
    }
}
