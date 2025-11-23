using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerBase : MonoBehaviour, IGetHit
{
    [Header("-----Base config-----")]
    [SerializeField] protected GunControllerBase _gun;
    [SerializeField] SlideBar _healthBar;
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] FlashEffect _flashEffect;
    [SerializeField] PlayerState _playerState;
    
    [SerializeField] protected float _hp, _armorPercent;
    [SerializeField] protected float _moveSpeed, _rotateSpeed;

    public enum PlayerState
    {
        IDLE,
        MOVE,
    }
    // Start is called before the first frame update
    void Start()
    {
        if (_rb == null)
            _rb = this.GetComponent<Rigidbody2D>();
    }
    public void Init()
    {

        _healthBar.SetMaxValue(_hp);
        _healthBar.UpdateValue(_hp);
        _playerState = PlayerState.IDLE;
        _flashEffect = this.GetComponentInChildren<FlashEffect>();
        if (_gun == null)
            _gun = this.GetComponentInChildren<GunControllerBase>();
        _gun.Init();
    }
    // Update is called once per frame
    void Update()
    {
        UpdateState();
    }
    private void FixedUpdate()
    {
        Move();
    }

    private void UpdateState()
    {
        _playerState = PlayerState.IDLE;
        if(_rb.velocity.x != 0)
            _playerState = PlayerState.MOVE;
    }
    private void Move()
    {
        _rb.velocity = this.transform.up * Input.GetAxis("Vertical") * _moveSpeed;

        this.transform.Rotate(new Vector3(0, 0, Input.GetAxis("Horizontal") * -_rotateSpeed));
    }
    public void GetHit(float dmg)
    {
        _hp = _hp - (dmg * (1 - _armorPercent / 100));
        _healthBar.UpdateValue(_hp);
        _flashEffect.Flash();
        if (_hp <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }

}
