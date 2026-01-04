using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerBase : MonoBehaviour, IGetHit, IDataPersistence
{
    [Header("-----Base config-----")]
    [SerializeField] protected GunControllerBase _gun;
    [SerializeField] SlideBar _healthBar;
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] PlayerState _playerState;

    private float _initialealth;
    [SerializeField] protected float _currentHealth, _armorPercent;
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

        _healthBar.SetMaxValue(_initialealth);
        _healthBar.UpdateValue(_currentHealth);
        _playerState = PlayerState.IDLE;
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
        _currentHealth = _currentHealth - (dmg * (1 - _armorPercent / 100));
        _healthBar.UpdateValue(_currentHealth);
        if (_currentHealth <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void LoadData(GameData data)
    {
        this._initialealth = data.initialHealth;
        this._currentHealth = data.currentHealth;
        this._armorPercent = data.armorPercentage;
        this._moveSpeed = data.moveSpeed;
        this.transform.position = data.playerPos;
        Quaternion quaternion = this.transform.rotation;
        quaternion.eulerAngles = new Vector3(data.playerRotation.x, data.playerRotation.y, data.playerRotation.z);
        this.transform.rotation = quaternion;
    }

    public void SaveData(ref GameData data)
    {
        data.currentHealth = this._currentHealth;
        data.armorPercentage = this._armorPercent;
        data.moveSpeed = this._moveSpeed;
        data.playerPos = this.transform.position;
        Quaternion quaternion = this.transform.rotation;
        data.playerRotation.Set(quaternion.eulerAngles.x, quaternion.eulerAngles.y, quaternion.eulerAngles.z);
    }
}
