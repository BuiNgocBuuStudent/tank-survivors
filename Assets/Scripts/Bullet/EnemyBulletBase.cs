using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletBase : BulletBase
{
    public override void Init(float speed, Vector2 movement)
    {
        if (_rb == null)
            _rb = this.GetComponent<Rigidbody2D>();

        _dmg = _baseDmg;
        this._speed = speed;
        this._movement = movement;
    }
    protected override void Boom(GameObject target)
    {
        this.gameObject.SetActive(false);
        _isCanGetHit = target.GetComponent<IGetHit>();
        _isCanGetHit?.GetHit(this._dmg);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        this.Boom(collision.gameObject);
    }
}
