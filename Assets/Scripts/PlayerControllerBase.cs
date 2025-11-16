using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerBase : MonoBehaviour, IGetHit
{
    [Header("-----Base config-----")]
    [SerializeField] protected GunControllerBase _gun;
    [SerializeField] SlideBar _healthBar;
    [SerializeField] Rigidbody2D _rb;

    
    [SerializeField] protected float _hp, _armorPercent;
    [SerializeField] protected float _moveSpeed, _rotateSpeed;

    // Start is called before the first frame update
    void Start()
    {
       
    }
    public void Init()
    {
        if (_rb == null)
            _rb = this.GetComponent<Rigidbody2D>();
        _healthBar.SetMaxValue(_hp);
        _healthBar.UpdateValue(_hp);
        if (_gun == null)
            _gun = this.GetComponentInChildren<GunControllerBase>();
        _gun.Init();
    }
    // Update is called once per frame
    void Update()
    {
        
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
    public void GetHit(float dmg)
    {
        _hp = _hp - (dmg * (1 - _armorPercent / 100));
        _healthBar.UpdateValue(_hp);

        if (_hp <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }

}
