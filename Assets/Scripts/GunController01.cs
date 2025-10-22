using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController01 : GunControllerBase
{


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _timer -= Time.deltaTime;
        this.transform.Rotate(new Vector3(0, 0, _rotateSpeed * Time.deltaTime));

        if (DetectTarget(_detectTargetRadius))
        {
            Vector2 dir = _enemy.position - this.transform.position;

            RaycastHit2D hit = Physics2D.Raycast(this.transform.position, dir, dir.magnitude);
            Debug.DrawRay(this.transform.position, dir, Color.red);

            Fire();
        }
    }
    protected override void Fire()
    {
        if (_timer >= 0)
            return;

        Vector2 movement = _enemy.transform.position - this.transform.position;

        float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg - 90;
        Quaternion quaternion = this.transform.rotation;
        quaternion.eulerAngles = new Vector3(0, 0, angle);
        this.transform.rotation = quaternion;

        BulletBase bullet = ObjectPooler.Instance.GetComp(_bulletPrefab);
        bullet.Init(_bulletSpeed, _bulletDmg, _bulletLifeTime, this.transform.up);
        bullet.transform.position = this.transform.position;
        bullet.transform.rotation = this.transform.rotation;
        bullet.gameObject.SetActive(true);

        _timer = _cooldownTime;
    }
}
