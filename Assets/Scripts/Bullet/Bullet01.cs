using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet01 : BulletBase
{
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
