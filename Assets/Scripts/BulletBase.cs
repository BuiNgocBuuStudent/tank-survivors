using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletBase : MonoBehaviour
{
    [Header("-----Base config-----")]
    [SerializeField] protected Rigidbody2D _rb;

    [SerializeField] protected float _speed, _dmg, _lifeTime;
    protected Vector2 _movement = Vector2.zero;
    protected Coroutine _deactivateWait = null;

    // Start is called before the first frame update
    void Start()
    {
    }
    public void Init(float speed, float dmg, float lifeTime, Vector2 movement)
    {
        this._speed = speed;
        this._dmg = dmg;
        this._lifeTime = lifeTime;
        this._movement = movement;

    }
    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        
    }
    protected abstract void Move();
    protected abstract IEnumerator RepeatLifeTime();
    public abstract void Boom(GameObject target);
}
