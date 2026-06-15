using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class EnemyControllerBase : MonoBehaviour, IGetHit
{
    private PlayerControllerBase _player;
    private EnemyManager _enemyManager;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] FlashEffect _flashEffect;
    [SerializeField] EnemyConfig _enemyDataBase;

    [SerializeField] float _currentHealth;
    /// <summary>EnemySlowState sẽ điều chỉnh field này</summary>
    [SerializeField] float _speedMultiplier = 1f;


    /// <summary>
    /// Phát ra khi enemy chết. Truyền vị trí và damage của đòn cuối
    /// Bullet/Skill subscribe vào event này để trigger chain effects
    /// </summary>
    public static event Action<Vector3, float> OnEnemyDeath;


    public void Init(Vector2 randomSpawnPos)
    {
        if (_rb == null)
            _rb = this.GetComponent<Rigidbody2D>();
        _flashEffect = this.GetComponentInChildren<FlashEffect>();
        _player = GameManager.Instance.Player;
        _enemyManager = GameManager.Instance.EnemyManager;

        _currentHealth = _enemyDataBase.intialHealth;
        this.transform.position = randomSpawnPos;
    }

    private void FixedUpdate()
    {
        ChaseTarget();
    }

    private void ChaseTarget()
    {
        Vector2 movement = _player.transform.position - this.transform.position;

        float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg + 90;
        Quaternion quaternion = this.transform.rotation;
        quaternion.eulerAngles = new Vector3(0, 0, angle);
        this.transform.rotation = quaternion;

        _rb.velocity = movement.normalized * (_enemyDataBase.moveSpeed * _speedMultiplier);
    }

    public void GetHit(float dmg)
    {
        if (gameObject.activeSelf)
            _flashEffect.Flash();

        _currentHealth -= dmg;
        if (_currentHealth <= 0)
        {
            OnEnemyDeath?.Invoke(this.transform.position, dmg);

            this.gameObject.SetActive(false);
            _flashEffect.ResetMaterial();
            _enemyManager.SpawnExpGem(this.transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IGetHit isCanGetHit = collision.gameObject.GetComponent<IGetHit>();
        if (isCanGetHit == null)
            return;
        isCanGetHit.GetHit(_enemyDataBase.damage);
    }

    /// <summary>
    /// Được gọi bởi EnemySlowState để giảm/restore tốc độ
    /// </summary>
    public void SetSpeedMultiplier(float multiplier)
    {
        _speedMultiplier = Mathf.Clamp(multiplier, 0.1f, 2f);
    }
}
