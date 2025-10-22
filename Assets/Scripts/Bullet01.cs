using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet01 : BulletBase
{
    // Start is called before the first frame update
    void Start()
    {
        _rb = this.GetComponent<Rigidbody2D>();

    }
    private void OnEnable()
    {
        _deactivateWait = StartCoroutine(RepeatLifeTime());
    }
    private void OnDisable()
    {
        if (_deactivateWait != null)
        {
            StopCoroutine(RepeatLifeTime());
            _deactivateWait = null;
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        Move();
    }
    public override void Boom(GameObject target)
    {
        this.gameObject.SetActive(false);
        IGetHit isCanGetHit = target.GetComponent<IGetHit>();

        if (isCanGetHit != null)
        {
            isCanGetHit.GetHit(this._dmg);
        }

    }

    protected override void Move()
    {
        _rb.velocity = _movement * _speed;
    }

    protected override IEnumerator RepeatLifeTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(_lifeTime);
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        this.Boom(collision.gameObject);
    }

}
