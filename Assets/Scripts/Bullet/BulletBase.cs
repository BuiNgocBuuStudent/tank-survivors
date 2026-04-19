using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BulletBase : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D _rb;
    protected Coroutine _deactivateWait = null;
    protected IGetHit _isCanGetHit;
    [SerializeField] protected LayerMask _targetMask;

    [Header("-----Base config-----")]
    [SerializeField] protected float _lifeTime;
    [SerializeField] protected float _dmg;
    [SerializeField] protected float _speed;
    [SerializeField] protected Vector2 _movement;


    public void Init(float speed, Vector2 movement)
    {
        if (_rb == null)
            _rb = this.GetComponent<Rigidbody2D>();
         
        this._speed = speed;
        this._movement = movement;
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
    private void FixedUpdate()
    {
        Move();
    }
    protected virtual void Move()
    {
        _rb.velocity = _movement.normalized * _speed;
    }

    protected IEnumerator RepeatLifeTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(_lifeTime);
            this.gameObject.SetActive(false);
        }
    }
    protected abstract void Boom(GameObject target);
}
