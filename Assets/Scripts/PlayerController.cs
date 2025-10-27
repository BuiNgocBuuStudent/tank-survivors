using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IGetHit
{
    public enum PlayerState
    {
        IDLE,
        MOVE
    }

    [SerializeField] GunControllerBase _gun;
    [SerializeField] HealthBar _healthBar;
    [SerializeField] PlayerState _playerState;
    Rigidbody2D _rb;

    [SerializeField] float _hp, _armorPercent;
    [SerializeField] float _moveSpeed, _rotateSpeed;

    // Start is called before the first frame update
    void Start()
    {
        _rb = this.GetComponent<Rigidbody2D>();
    }
    public void Init()
    {
        _healthBar.SetMaxHealth(_hp);
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
    void Move()
    {
        _rb.velocity = this.transform.up * Input.GetAxis("Vertical") * _moveSpeed;

        this.transform.Rotate(new Vector3(0, 0, Input.GetAxis("Horizontal") * -_rotateSpeed));
    }
    private void UpdateState()
    {
        if (_rb.velocity.y != 0)
            _playerState = PlayerState.MOVE;
        else
            _playerState = PlayerState.IDLE;
    }

    public void GetHit(float dmg)
    {
        _hp = _hp - (dmg * (1 - _armorPercent / 100));
        _healthBar.SetHeath(_hp);

        if (_hp <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }

}
