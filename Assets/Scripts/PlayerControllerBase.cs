using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerBase : MonoBehaviour, IGetHit, IDataPersistence
{
    [Header("-----Base config-----")]
    [SerializeField] Rigidbody2D _rb;

    [SerializeField] protected GunControllerBase _gun;
    [SerializeField] FlashEffect _flashEffect;
    [SerializeField] PlayerState _playerState;
    [SerializeField] AnimationController _animController;
    private Coroutine _rechargeCoroutine;

    protected float _initialHealth, _initialSpeed, _accelerateSpeed, _initialEnergy;
    [SerializeField] protected float _currentHealth, _currentEnergy;
    [SerializeField] protected float _moveSpeed, _rotateSpeed, _armorPercent;
    [SerializeField] protected bool _isFullEnergy;
    // true khi năng lượng vừa cạn — chặn tăng tốc cho đến khi nạp lại ≥15%
    // Tránh bug giữa MOVE/ACCELERATE do recharge coroutine trả lại energy từng chút
    private bool _isEnergyDepleted;

    [Header("-----Energy Config-----")]
    [Tooltip("Năng lượng tiêu thụ mỗi giây khi tăng tốc")]
    [SerializeField] protected float _drainRate = 5f;
    [Tooltip("Tốc độ nạp lại năng lượng mỗi giây sau khi ngừng tăng tốc")]
    [SerializeField] protected float _rechargeRate = 2.5f;
    [Tooltip("Giây chờ sau khi ngừng tăng tốc trước khi bắt đầu nạp")]
    [SerializeField] protected float _rechargeDelay = 2f;

    public float dmgMult;
    protected float _movement;

    public event Action<float> OnMaxHealthSet;
    public event Action<float> OnHealthChanged;

    public event Action<float> OnMaxEnergySet;
    public event Action<float> OnEnergyChanged;

    public enum PlayerState
    {
        IDLE,
        MOVE,
        ACCELERATE
    }

    void Start()
    {

    }
    public void Init()
    {
        Debug.LogWarning("Calling");
        if (_rb == null)
            _rb = this.GetComponent<Rigidbody2D>();
        if (_animController == null)
            _animController = this.GetComponentInChildren<AnimationController>();
        if (_gun == null)
            _gun = this.GetComponentInChildren<GunControllerBase>();
        if (_flashEffect == null)
            _flashEffect = this.GetComponentInChildren<FlashEffect>();

        _isFullEnergy = true;
        _isEnergyDepleted = false;
        _playerState = PlayerState.IDLE;
        _gun.Init();
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        _movement = Input.GetAxis("Vertical");
        Move();
        Accelerate();

        UpdateState();
        _animController.UpdatePlayerAnim(_playerState);

        CheckEnergy();

        OnEnergyChanged?.Invoke(_currentEnergy);
    }


    private void UpdateState()
    {
        _playerState = PlayerState.IDLE;
        if (_rb.velocity.x != 0)
            _playerState = PlayerState.MOVE;
        if (_moveSpeed == _accelerateSpeed)
            _playerState = PlayerState.ACCELERATE;
    }

    private void Move()
    {
        _rb.velocity = this.transform.up * _movement * _moveSpeed;
        this.transform.Rotate(new Vector3(0, 0, Input.GetAxis("Horizontal") * -_rotateSpeed));
    }

    private void Accelerate()
    {
        _moveSpeed = _initialSpeed;

        if (_currentEnergy <= 0)
        {
            _currentEnergy = 0;
            _isEnergyDepleted = true;
            return;
        }

        // khi đã cạn năng lượng, chờ nạp lại >= 15% mới cho phép tăng tốc trở lại
        if (_isEnergyDepleted)
        {
            if (_currentEnergy >= _initialEnergy * 0.15f)
                _isEnergyDepleted = false;
            else
                return;
        }

        if (Input.GetKey(KeyCode.Space) && _movement > 0)
        {
            _moveSpeed = _accelerateSpeed;
            _currentEnergy -= _drainRate * Time.deltaTime;
            _isFullEnergy = false;
        }
    }

    private void CheckEnergy()
    {
        if (_isFullEnergy || _currentEnergy >= _initialEnergy)
        {
            Debug.Log("Full energy");
            _rechargeCoroutine = null;
            return;
        }

        if (_currentEnergy < _initialEnergy && _rechargeCoroutine == null)
        {
            Debug.Log("Start recharge");
            _rechargeCoroutine = StartCoroutine(RechargeEnergy());
        }
    }

    private IEnumerator RechargeEnergy()
    {
        // Chờ một khoảng thời gian sau khi ngừng tăng tốc rồi mới bắt đầu nạp
        yield return new WaitForSeconds(_rechargeDelay);


        while (_currentEnergy < _initialEnergy)
        {
            _currentEnergy = Mathf.Min(_currentEnergy + _rechargeRate * Time.deltaTime,
                                       _initialEnergy);
            yield return null; // Chờ frame tiếp theo (thực hiện đúng 1 tick/frame)
        }

        _currentEnergy = _initialEnergy;
        _rechargeCoroutine = null;
        _isFullEnergy = true;
    }

    public void GetHit(float dmg)
    {
        Debug.LogError(dmg);
        if (gameObject.activeSelf)
            _flashEffect.Flash();

        _currentHealth = _currentHealth - (dmg * (1 - _armorPercent / 100));

        OnHealthChanged?.Invoke(_currentHealth);

        if (_currentHealth <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// ★ HÀM CHÍNH khi chuyển scene: đọc PlayerSessionData DTO → ghi đè stats,
    /// phát events để HUDController cập nhật SlideBar, apply skills vào gun.
    /// Phải gọi SAU Init() để _gun đã được resolve.
    /// </summary>
    public void ApplySessionData(PlayerSessionData sessionData)
    {
        if (sessionData == null)
        {
            Debug.LogError("[PlayerControllerBase] sessionData là null!");
            return;
        }

        // Ghi đè stats
        _initialHealth = sessionData.health;
        _currentHealth = sessionData.health;
        _initialEnergy = sessionData.energy;
        _currentEnergy = sessionData.energy;
        _armorPercent  = sessionData.armor;
        dmgMult        = sessionData.dmgMult;

        OnMaxHealthSet?.Invoke(_initialHealth);
        OnMaxEnergySet?.Invoke(_initialEnergy);

        OnHealthChanged?.Invoke(_currentHealth);
        OnEnergyChanged?.Invoke(_currentEnergy);

        // Apply skills vào gun
        if (_gun != null)
            _gun.ApplySkills(sessionData.activeSkills);

        Debug.LogError($"[PlayerControllerBase] ApplySessionData: HP={_initialHealth}, " +
                  $"EN={_initialEnergy}, AR={_armorPercent}, DMG={dmgMult}, " +
                  $"Skills=[{string.Join(", ", sessionData.activeSkills)}]");
    }


    #region Save/Load Data
    public void LoadData(GameData data)
    {
        this._initialHealth = data.initialHealth;
        this._currentHealth = data.currentHealth;

        this._armorPercent = data.armorPercentage;

        this._initialEnergy = data.initialEnergy;
        this._currentEnergy = data.currentEnergy;

        this._initialSpeed = data.initialSpeed;
        this._accelerateSpeed = data.accelerateSpeed;

        this.dmgMult = data.dmgMult;

        this.transform.position = new Vector3(data.playerPosX, data.playerPosY, data.playerPosZ);

        Quaternion quaternion = this.transform.rotation;
        quaternion.eulerAngles = new Vector3(data.playerRotationX, data.playerRotationY, data.playerRotationZ);
        this.transform.rotation = quaternion;
    }

    public void SaveData(GameData data)
    {
        data.initialHealth = this._initialHealth;
        data.currentHealth = this._currentHealth;

        data.armorPercentage = this._armorPercent;

        data.moveSpeed = this._moveSpeed;

        data.initialEnergy = this._initialEnergy;
        data.currentEnergy = this._currentEnergy;

        data.dmgMult = this.dmgMult;

        data.playerPosX = this.transform.position.x;
        data.playerPosY = this.transform.position.y;
        data.playerPosZ = this.transform.position.z;

        Quaternion quaternion = this.transform.rotation;
        data.playerRotationX = quaternion.eulerAngles.x;
        data.playerRotationY = quaternion.eulerAngles.y;
        data.playerRotationZ = quaternion.eulerAngles.z;
    }
    #endregion
}
