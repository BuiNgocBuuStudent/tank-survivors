using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController02 : GunControllerBase
{
    [Header("Shotgun config")]
    [SerializeField] int _numberBullet;
    [SerializeField] float _attackRange;
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

        float centerAngle = Mathf.Atan2(_movement.y, _movement.x) * Mathf.Rad2Deg;
        float anglePerBullet = _attackRange / (_numberBullet - 1);
        float startAngle = centerAngle + _attackRange / 2;

        _timer = _cooldownTime;

        for (int i = 0; i < _numberBullet; i++)
        {
            float bulletAngle = (startAngle - anglePerBullet * i) * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(bulletAngle), Mathf.Sin(bulletAngle));

            BulletBase bullet = ObjectPooler.Instance.GetComp(_bulletPrefab);
            bullet.Init(_bulletSpeed, direction);
            bullet.transform.position = this.transform.position;
            bullet.transform.rotation = this.transform.rotation;
            bullet.gameObject.SetActive(true);
        }
    }

}
