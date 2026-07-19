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
    protected override void TriggerHit(GameObject target)
    {
        base.TriggerHit(target);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        this.TriggerHit(collision.gameObject);
    }
}
