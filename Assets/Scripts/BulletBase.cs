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
    public void Init(Vector2 movement)
    {
        if (_rb == null)
            _rb = this.GetComponent<Rigidbody2D>();
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
