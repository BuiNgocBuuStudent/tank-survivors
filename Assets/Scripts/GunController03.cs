using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController03 : GunControllerBase
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(new Vector3(0, 0, _rotateSpeed * Time.deltaTime));

        this._timer -= Time.deltaTime;

        if (DetectTarget(_detectTargetRadius))
        {
            Vector2 dir = _enemy.transform.position - this.transform.position;

            RaycastHit2D hit = Physics2D.Raycast(this.transform.position, dir, dir.magnitude);
            Debug.DrawRay(this.transform.position, dir, Color.red);

            Fire();
        }
    }

    protected override void Fire()
    {
        if (_timer >= 0 || !AimTarget())
            return;

        _timer = _cooldownTime;

        BulletBase bullet = ObjectPooler.Instance.GetComp(_bulletPrefab);
        bullet.Init(_bulletSpeed, _movement);
        bullet.transform.position = this.transform.position;
        bullet.transform.rotation = this.transform.rotation;
        bullet.gameObject.SetActive(true);
    }

}
