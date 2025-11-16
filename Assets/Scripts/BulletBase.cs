using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BulletBase : MonoBehaviour
{
    
    protected Rigidbody2D _rb;

    [SerializeField] protected float _lifeTime, _dmg;
    protected float _speed;
    protected Vector2 _movement = Vector2.zero;
    protected Coroutine _deactivateWait = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Init(float speed, Vector2 movement)
    {
        if (_rb == null)
            _rb = this.GetComponent<Rigidbody2D>();

        this._speed = speed;
        this._movement = movement;
    }
    // Update is called once per frame
    void Update()
    {
        
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
        _rb.velocity = _movement * _speed;
    }

    protected IEnumerator RepeatLifeTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(_lifeTime);
            this.gameObject.SetActive(false);
        }
    }
    public abstract void Boom(GameObject target);
}
