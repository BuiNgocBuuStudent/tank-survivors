using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerBase : MonoBehaviour, IGetHit, IDataPersistence
{
    [Header("-----Base config-----")]
    [SerializeField] protected GunControllerBase _gun;
    [SerializeField] SlideBar _healthBar;
    [SerializeField] SlideBar _energyBar;
    [SerializeField] FlashEffect _flashEffect;
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] PlayerState _playerState;
    [SerializeField] AnimationController _animController;
    private Coroutine _rechargeCoroutine;

    protected float _initialHealth, _normalSpeed, _accelerateSpeed, _initialEnergy;
    [SerializeField] protected float _currentHealth, _currentEnergy;
    [SerializeField] protected float _moveSpeed, _rotateSpeed, _armorPercent;
    [SerializeField] protected bool _isFullEnergy;
    protected float _movement;
    public enum PlayerState
    {
        IDLE,
        MOVE,
        ACCELERATE
    }
    // Start is called before the first frame update
    void Start()
    {

    }
    public void Init()
    {
        if (_rb == null)
            _rb = this.GetComponent<Rigidbody2D>();
        if (_animController == null)
            _animController = this.GetComponentInChildren<AnimationController>();
        if (_gun == null)
            _gun = this.GetComponentInChildren<GunControllerBase>();
        if(_flashEffect == null)
            _flashEffect = this.GetComponentInChildren<FlashEffect>();

        _isFullEnergy = true;
        _healthBar.SetMaxValue(_initialHealth);
        _healthBar.UpdateValue(_currentHealth);

        _energyBar.SetMaxValue(_initialEnergy);
        _energyBar.UpdateValue(_currentEnergy);

        _playerState = PlayerState.IDLE;
        _gun.Init();
    }
    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        _movement = Input.GetAxis("Vertical");
        _moveSpeed = _normalSpeed;
        Move();
        Accelerate();

        UpdateState();
        _animController.UpdatePlayerAnim(_playerState);

        CheckEnergy();

        _energyBar.UpdateValue(_currentEnergy);

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
        if (_currentEnergy <= 0)
        {
            _currentEnergy = 0;
            return;
        }

        if (Input.GetKey(KeyCode.Space) && _movement > 0)
        {
            _moveSpeed = _accelerateSpeed;
            _currentEnergy -= 5 * Time.deltaTime;
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
        while (_currentEnergy < _initialEnergy)
        {
            Debug.Log("Recharging");
            yield return new WaitForSeconds(Time.deltaTime);
            _currentEnergy += Time.deltaTime;
        }

        if (_initialEnergy - _currentEnergy < 0.2f)
            _currentEnergy = _initialEnergy;

        _rechargeCoroutine = null;
        _isFullEnergy = true;
    }


    public void GetHit(float dmg)
    {
        if (gameObject.activeSelf)
            _flashEffect.Flash();

        _currentHealth = _currentHealth - (dmg * (1 - _armorPercent / 100));
        _healthBar.UpdateValue(_currentHealth);
        if (_currentHealth <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void LoadData(GameData data)
    {
        this._initialHealth = data.initialHealth;
        this._currentHealth = data.currentHealth;

        this._armorPercent = data.armorPercentage;

        this._initialEnergy = data.initialEnergy;
        this._currentEnergy = data.currentEnergy;

        this._normalSpeed = data.normalSpeed;
        this._accelerateSpeed = data.accelerateSpeed;

        this.transform.position = data.playerPos;
        Quaternion quaternion = this.transform.rotation;
        quaternion.eulerAngles = new Vector3(data.playerRotation.x, data.playerRotation.y, data.playerRotation.z);
        this.transform.rotation = quaternion;
    }

    public void SaveData(GameData data)
    {
        data.currentHealth = this._currentHealth;
        data.armorPercentage = this._armorPercent;
        data.moveSpeed = this._moveSpeed;
        data.currentEnergy = this._currentEnergy;
        data.playerPos = this.transform.position;
        Quaternion quaternion = this.transform.rotation;
        data.playerRotation.Set(quaternion.eulerAngles.x, quaternion.eulerAngles.y, quaternion.eulerAngles.z);
    }
}
