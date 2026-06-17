using System.Collections;
using UnityEngine;

public abstract class BulletBase : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D _rb;
    protected Coroutine _deactivateWait = null;
    protected IGetHit _isCanGetHit;
    [SerializeField] protected LayerMask _targetMask;

    [Header("-----Base config-----")]
    [SerializeField] protected float _lifeTime;
    [SerializeField] protected float _baseDmg;
    protected float _dmg;
    protected float _speed;
    protected Vector2 _movement;
    private float _damageMultiplier = 1f;


    public void Init(float speed, Vector2 movement)
    {
        if (_rb == null)
            _rb = this.GetComponent<Rigidbody2D>();

        _dmg = _baseDmg * GameManager.Instance.Player.dmgMult;

        // Reset mỗi lần lấy từ pool
        _damageMultiplier = 1f;
        this.transform.localScale = Vector3.one;

        this._speed = speed;
        this._movement = movement;
    }

    /// <summary>
    /// Nhân thêm hệ số damage. Gọi SAU Init().
    /// VD: Slug Round (×2), Cluster Fragment (×0.3)
    /// </summary>
    public void SetDamageMultiplier(float multiplier)
    {
        _damageMultiplier = multiplier;
        _dmg = _baseDmg * GameManager.Instance.Player.dmgMult * _damageMultiplier;
    }
    protected virtual void OnEnable()
    {
        _deactivateWait = StartCoroutine(RepeatLifeTime());
    }
    protected virtual void OnDisable()
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

    protected virtual IEnumerator RepeatLifeTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(_lifeTime);
            this.gameObject.SetActive(false);
        }
    }
    protected abstract void Boom(GameObject target);
}
