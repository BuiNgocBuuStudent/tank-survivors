using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController03 : GunControllerBase
{

    // Update is called once per frame
    void Update()
    {
        Fire();
    }

    protected override void Fire()
    {
        if (_timer >= 0)
            return;

        _timer = _cooldownTime;

        BulletBase bullet = ObjectPooler.Instance.GetComp(_bulletPrefab);
        bullet.Init(_bulletSpeed, this.transform.up);
        bullet.transform.SetPositionAndRotation(this.transform.position, this.transform.rotation);
        bullet.gameObject.SetActive(true);
    }

}
